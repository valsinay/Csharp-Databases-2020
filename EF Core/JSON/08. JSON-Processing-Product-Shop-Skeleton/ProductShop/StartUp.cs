using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();

            // ResetDatabase(db);

            var userPath = File.ReadAllText("../../../Datasets/users.json");
            var productsPath = File.ReadAllText("../../../Datasets/products.json");
            var categoriesPath = File.ReadAllText("../../../Datasets/categories.json");
            var categoriesProductsPath = File.ReadAllText("../../../Datasets/categories-products.json");

            var ResultDirectoryPath = "../../../Datasets/Results";

            //ImportUsers(db, userPath);
            //ImportProducts(db, productsPath);
            //ImportCategories(db, categoriesPath);
            //ImportCategoryProducts(db, categoriesProductsPath);

            string json = GetUsersWithProducts(db);

            if (!Directory.Exists(ResultDirectoryPath))
            {
                Directory.CreateDirectory(ResultDirectoryPath);
            }

            File.WriteAllText(ResultDirectoryPath + "/users-and-products.json", json);

        }

        public static void ResetDatabase(ProductShopContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfully deleted!");
            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created!");
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {

            var users = JsonConvert.DeserializeObject<User[]>(inputJson);

            int count = users.Count();
            context.Users.AddRange(users);

            context.SaveChanges();
            return $"Successfully imported {count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            int productsCount = products.Count();

            context.Products.AddRange(products);

            context.SaveChanges();

            return $"Successfully imported {productsCount}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<Category[]>(inputJson)
                .Where(x => x.Name != null)
                .ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriesProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();
            return $"Successfully imported {categoriesProducts.Length}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new
                {
                    name = x.Name,
                    price = x.Price,
                    seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .ToArray();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;


        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(b => b.Buyer != null))
                .OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = x.ProductsSold
                    .Where(p => p.Buyer != null)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyerFirstName = p.Buyer.FirstName,
                        buyerLastName = p.Buyer.LastName
                    }).ToList()
                }).ToList();

            string json = JsonConvert.SerializeObject(users, Formatting.Indented);

            return json;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count(),
                    averagePrice = c.CategoryProducts.Average(cp => cp.Product.Price).ToString("f2"),
                    totalRevenue = c.CategoryProducts.Select(cp => cp.Product.Price).Sum().ToString("f2")
                })
                .OrderByDescending(c => c.productsCount)
                .ToList();

            var json = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return json;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(b => b.Buyer != null))
                .Select(x => new    
                {
                    lastName = x.LastName,
                    age = x.Age,
                    soldProducts = new
                    {
                        count = x.ProductsSold
                        .Count(b => b.Buyer != null),
                        products = x.ProductsSold
                        .Where(b=>b.Buyer != null)
                          .Select(ps => new
                          {
                              name = ps.Name,
                              price = ps.Price
                          }).ToArray()
                    }
                }).OrderByDescending(x=>x.soldProducts.count)
                .ToArray();

            var resultObj = new
            {
                usersCount = users.Length,
                users= users
            };

            string json = JsonConvert.SerializeObject(resultObj, Formatting.Indented,
               new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return json;
        }
    }
}