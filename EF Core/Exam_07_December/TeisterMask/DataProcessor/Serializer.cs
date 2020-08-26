namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var xmlSerializer = new XmlSerializer(typeof(ExportProjectDTO[]), new XmlRootAttribute("Projects"));

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty,string.Empty);


            using (StringWriter stringWriter = new StringWriter(sb))
            {


                var projectShit = context.Projects.ToList();

                var projects = context.Projects
                    .Where(p => p.Tasks.Count > 0)
                    .Select(x => new ExportProjectDTO()
                    {
                        Name = x.Name,
                        TasksCount = x.Tasks.Count,
                        HasEndDate = x.DueDate.HasValue ? "Yes" : "No",
                        Tasks = x.Tasks
                        .Select(t => new ExportTaskDTO()
                        {
                            Name = t.Name,
                            LabelType = t.LabelType.ToString()
                        })
                        .OrderBy(t => t.Name)
                        .ToArray()
                    })
                    .OrderByDescending(x => x.TasksCount).ThenBy(x => x.Name)
                    .ToArray();


                xmlSerializer.Serialize(stringWriter, projects,namespaces);
            }
    

            return sb.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {

            var employees = context.Employees
                .ToArray()
                .Where(e => e.EmployeesTasks.Any(et=>et.Task.OpenDate >= date))
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                     .Where(et=> et.Task.OpenDate >= date)
                     .OrderByDescending(x => x.Task.DueDate).ThenBy(x => x.Task.Name)
                     .Select(t => new
                    {
                        TaskName = t.Task.Name,
                        OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture)     ,
                        DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = t.Task.LabelType.ToString(),
                        ExecutionType = t.Task.ExecutionType.ToString()
                    }).ToArray()    
                })
                .OrderByDescending(e => e.Tasks.Length).ThenBy(e => e.Username)
                .Take(10)
                .ToArray(); 

            string emplSerializer = JsonConvert.SerializeObject(employees, Formatting.Indented);

            return emplSerializer;  
        }
    }
}