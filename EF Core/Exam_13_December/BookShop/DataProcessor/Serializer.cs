namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    AuthorName = a.FirstName + " " + a.LastName,
                    Books = a.AuthorsBooks.Select(ab => new
                    {
                        BookName = ab.Book.Name,
                        BookPrice = ab.Book.Price.ToString("f2")
                    })
                    .OrderByDescending(ab => ab.BookPrice)
                    .ToArray()
                })
                .ToArray()
                .OrderByDescending(a => a.Books.Length)
                .ThenBy(a => a.AuthorName)
                .ToArray();


            var json = JsonConvert.SerializeObject(authors, Formatting.Indented);

            return json;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var xmlSerializer = new XmlSerializer(typeof(ExportBookDTO[]), new XmlRootAttribute("Books"));

            var oldestBooks = context.Books
                .Where(b => b.PublishedOn < date && b.Genre.ToString() == "Science")
                .Select(b => new ExportBookDTO()
                {
                    Name = b.Name,
                    Date = b.PublishedOn.ToString("d", CultureInfo.InvariantCulture),
                    Pages = b.Pages
                })
                .ToArray()
                .OrderByDescending(b => b.Pages).ThenByDescending(b => b.Date)
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();

            namespaces.Add(string.Empty, string.Empty);


            var writer = new StringWriter(sb);

            using (writer)
            {

               xmlSerializer.Serialize(writer, oldestBooks, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}