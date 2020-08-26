using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());

            var mapper = new Mapper(config);

            CarDealerContext context = new CarDealerContext();

            //ResetDatabase(context);
            string supplierPath = File.ReadAllText("../../../Datasets/suppliers.xml");
            string partPath = File.ReadAllText("../../../Datasets/parts.xml");
            string carsPath = File.ReadAllText("../../../Datasets/cars.xml");
            string customerPath = File.ReadAllText("../../../Datasets/customers.xml");
            string salesPath = File.ReadAllText("../../../Datasets/sales.xml");

            //ImportSuppliers(context, supplierPath);
            //Console.WriteLine(ImportParts(context, partPath));
            Console.WriteLine(GetTotalSalesByCustomer(context));
        }

        public static void ResetDatabase(CarDealerContext db)
        {
            db.Database.EnsureDeleted();

            Console.WriteLine("Database successfully deleted!");

            db.Database.EnsureCreated();
            Console.WriteLine("Database successfully created!");
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSupplierDTO[]), new XmlRootAttribute("Suppliers"));

            var deserializier = (ImportSupplierDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Supplier> suppliers = new List<Supplier>();

            foreach (var supplierDTO in deserializier)
            {
                //var supplier = Mapper.Map<Supplier>(supplierDTO);

                // suppliers.Add(supplier);
            }

            //TODO: Add in database
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count}";

        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {


            var serializer = new XmlSerializer(typeof(ImportPartDTO[]), new XmlRootAttribute("Parts"));

            var deserializer = (ImportPartDTO[])serializer.Deserialize(new StringReader(inputXml));



            deserializer = deserializer.Where(p => context.Suppliers.Any(s => s.Id == p.SupplierId)).ToArray();
            // var parts = Mapper.Map<Part[]>(deserializer);

            //context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported";

        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportCarDTO[]), new XmlRootAttribute("Cars"));

            var carDtos = (ImportCarDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));


            List<Car> cars = new List<Car>();
            List<PartCar> partCars = new List<PartCar>();

            foreach (var carDto in carDtos)
            {
                var car = new Car
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };


                cars.Add(car);

                var parts =
                    carDto.Parts
                    .Where(pdto => context.Parts.Any(p => p.Id == pdto.Id))
                    .Select(p => p.Id)
                    .Distinct();


                foreach (var partId in parts)
                {
                    var partCar = new PartCar()
                    {
                        PartId = partId,
                        Car = car
                    };

                    partCars.Add(partCar);
                }


            }



            context.Cars.AddRange(cars);
            context.PartCars.AddRange(partCars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count()}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCustomerDTO[]), new XmlRootAttribute("Customers"));

            var customersDto = (ImportCustomerDTO[])serializer.Deserialize(new StringReader(inputXml));

            List<Customer> customers = new List<Customer>();

            foreach (var customer in customersDto)
            {
                var newCustomer = new Customer()
                {
                    Name = customer.Name,
                    BirthDate = customer.BirthDate,
                    IsYoungDriver = customer.IsYoungDriver
                };

                customers.Add(newCustomer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();


            return $"Successfully imported {customers.Count()}";


        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportSaleDTO[]), new XmlRootAttribute("Sales"));

            var salesDtos = (ImportSaleDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            List<Sale> sales = new List<Sale>();

            salesDtos = salesDtos.Where(x => context.Cars.Any(c => c.Id == x.CarId)).ToArray();

            foreach (var saleDto in salesDtos)
            {
                var newSale = new Sale()
                {
                    CarId = saleDto.CarId,
                    CustomerId = saleDto.CustomerId,
                    Discount = saleDto.Discount
                };

                sales.Add(newSale);
            }

            context.Sales.AddRange(sales);
            //TODO
            // context.SaveChanges();

            return $"Successfully imported {sales.Count()}";


        }

        public static string GetCarsWithDistance(CarDealerContext context)

        {

            var cars = context.Cars
                .Select(c => new ExportCarDistanceDTO()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make).ThenBy(c => c.Model)
                .Take(10)
                .ToList();

            var xmlSerializer = new XmlSerializer(cars.GetType(), new XmlRootAttribute("cars"));


            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter())
            {

                xmlSerializer.Serialize(writer, cars,namespaces);

                return writer.ToString();
            }

        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {

            var bmwCars = context.Cars
                .Where(x => x.Make == "BMW")
                .Select(x => new ExportBmwCarsDTO()
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportBmwCarsDTO[]),new XmlRootAttribute("cars"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var writer = new StringWriter();

            using (writer)
            {
                xmlSerializer.Serialize(writer, bmwCars,namespaces);
            }

            return writer.ToString();

        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new LocalSupplierDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count()
                })
              .ToArray();

            StringBuilder sb = new StringBuilder();
            var xmlSerializer = new XmlSerializer(typeof(LocalSupplierDTO[]),new XmlRootAttribute("suppliers"));


            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add("", "");

            using(var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, suppliers, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(x => x.Sales.Any())
                .Select(x => new TotalSalesCustomerDTO()
                {
                    Name = x.Name,
                    BoughtCars = x.Sales.Count()
                    //SpentMoney = x.Sales.Sum((s => s.Car.PartCars.Sum(p => p.Part.Price)))
                })
                //.OrderByDescending(x => x.SpentMoney)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(TotalSalesCustomerDTO[]), new XmlRootAttribute("sales"));

            StringBuilder sb = new StringBuilder();
            var writer = new StringWriter(sb);

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (writer)
            {
                xmlSerializer.Serialize(writer, customers, namespaces);
            }

            return sb.ToString().TrimEnd();

        }

        //public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        //{

        //}
    }
}