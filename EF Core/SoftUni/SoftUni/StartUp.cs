using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext context = new SoftUniContext();

            string result = RemoveTown(context);
            Console.WriteLine(result);
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {

            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Select(x => new
                {
                    x.FirstName,
                    x.MiddleName,
                    x.LastName,
                    x.JobTitle,
                    x.Salary
                })
                .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
            }

            return sb.ToString();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {

            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(x => x.Salary > 50000)
                .Select(x => new

                {
                    x.FirstName,
                    x.Salary
                })
                .OrderBy(x => x.FirstName)
                .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:f2}");
            }

            return sb.ToString();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .Where(x => x.Department.Name == "Research and Development")
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.Department,
                    x.Salary
                })
                .OrderBy(x => x.Salary).ThenByDescending(x => x.FirstName)
                .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:f2}");
            }

            return sb.ToString();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();



            var employee = context.Employees.
                First(x => x.LastName == "Nakov");

            employee.Address = new Address { AddressText = "Vitoshka 15", TownId = 4 };
            context.SaveChanges();

            var employees = context.Employees
            .OrderByDescending(x => x.AddressId)
            .Take(10)
            .Select(x => x.Address.AddressText)
            .ToList();



            foreach (var e in employees)
            {
                sb.AppendLine($"{e}");
            }

            return sb.ToString().TrimEnd();

        }


        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees.
                Where(x => x.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003)).
                Take(10).
                Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(ep => new
                    {
                        StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt"),
                        EndDate = ep.Project.EndDate.HasValue ?
                        ep.Project.EndDate.Value.ToString
                        ("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished",
                        ProjectName = ep.Project.Name
                    }).ToList()
                })
                .ToList();

            foreach (var e in employees)
            {

                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                foreach (var ep in e.Projects)
                {
                    sb.AppendLine($"--{ep.ProjectName} - {ep.StartDate} - {ep.EndDate}");
                }

            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context.Employees
                .Where(x => x.EmployeeId == 147)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.JobTitle,

                    EmployeesProjects = x.EmployeesProjects
                  .OrderBy(x => x.Project.Name)
                  .Select(ep => new 
                  { 
                    ep.Project.Name
                   })
                  .ToList()
                })
                .SingleOrDefault();

            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var p in employee.EmployeesProjects)
            {
                sb.AppendLine($"{p.Name}");
            }

            return sb.ToString().TrimEnd();
        }

         public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .Select(d => new
                {
                    d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees
                    .Select(e => new
                    {
                        EmployeeFirstName = e.FirstName,
                        EmployeeLastName = e.LastName,
                        EmployeeJobTitle = e.JobTitle
                    })
                    .OrderBy(e => e.EmployeeFirstName).ThenBy(e => e.EmployeeLastName)
                    .ToList()
                })
                .ToList()
                 .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name);
            
            foreach (var d in departments)
            {
                sb.AppendLine($"{d.Name} - {d.ManagerFirstName} {d.ManagerLastName}");

                foreach (var e in d.Employees)
                {
                    sb.AppendLine($"{e.EmployeeFirstName} {e.EmployeeLastName} - {e.EmployeeJobTitle}");
                }

            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var projects = context.Projects.OrderByDescending(x=>x.StartDate)
                .Take(10)
                .OrderBy(x => x.Name)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    Startdate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                })
                .ToList();

            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine(p.Startdate);
                
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)

        {
            StringBuilder sb = new StringBuilder();

            IQueryable<Employee> employeesToIncrease = context.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" ||
                       e.Department.Name == "Marketing" || e.Department.Name == "Information Services");

            foreach (Employee e in employeesToIncrease)
            {
                e.Salary  = e.Salary * 1.12M;
            }

            context.SaveChanges();

            var employees = employeesToIncrease
                .OrderBy(e=>e.FirstName).ThenBy(e=>e.LastName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    Salary=e.Salary
                }).ToList();


            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();
        }


         public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
               .Where(e => e.FirstName.StartsWith("Sa"))
                 .OrderBy(e => e.FirstName).ThenBy(e => e.LastName)
               .Select(e => new
               {
                   e.FirstName,
                   e.LastName,
                   e.JobTitle,
                   e.Salary
               })
               .ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:f2})");
            }

            return sb.ToString().TrimEnd();

              
        }

         public static string DeleteProjectById(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var project = context.Projects
                .Where(p => p.ProjectId == 2).Single();

            var referenceProject = context.EmployeesProjects
                .Where(p => p.ProjectId == 2).ToList();


            foreach (var rp in referenceProject)
            {

            context.EmployeesProjects.Remove(rp);
            }
            context.Projects.Remove(project);

            context.SaveChanges();  
            var projects = context.Projects
                .Take(10)
                .Select(p => new { p.Name } );

            foreach (var p in projects)
            {
                sb.AppendLine($"{p.Name}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            int count = 0;
            
            Town town = context.Towns
                .First(t => t.Name == "Seattle");

            IQueryable<Address> addresses = context.Addresses
                .Where(a => a.TownId == town.TownId);


            IQueryable<Employee> employees = context.Employees.
                Where(e => addresses.Any(a=> a.AddressId == e.AddressId));

            foreach (var e in employees)
            {
                e.AddressId = null;
            }

            foreach (var a in addresses)
            {
                context.Addresses.Remove(a);
                count += 1;
            }

            context.Towns.Remove(town);
            context.SaveChanges();

            return $"{count} addresses in Seattle were deleted";
        }
    }
}
