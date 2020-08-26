namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //int input =int.Parse(Console.ReadLine());
            int result = RemoveBooks(db);

            Console.WriteLine(result);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = new List<string>();
            command = command.ToLower();

            if(command == "minor")
            {
                 books = context.Books.Where(x => x.AgeRestriction == AgeRestriction.Minor)
                    .OrderBy(b=>b.Title)
                    .Select(b=>b.Title)
                   .ToList();
            }
            else if(command == "teen")
            {

                books = context.Books.Where(x => x.AgeRestriction == AgeRestriction.Teen)
                   .OrderBy(b => b.Title)
                   .Select(b => b.Title)
                  .ToList();
            }
            else
            {

                books = context.Books.Where(x => x.AgeRestriction == AgeRestriction.Adult)
                   .OrderBy(b => b.Title)
                   .Select(b => b.Title)
                  .ToList();
            }

            return String.Join(Environment.NewLine, books);

            
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.EditionType== EditionType.Gold && b.Copies < 5000)
                .OrderBy(b=>b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                }).ToList();

            foreach (var b in books)
            {
                sb.AppendLine($"{b.Title} - ${b.Price:f2}");
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] genres = input.Split(' ',
                StringSplitOptions.RemoveEmptyEntries)
                .Select(c=>c.ToLower())
                .ToArray();

            List<string> finalLibrary = new List<string>();

            foreach (var genre in genres)
            {
                  List<string> books = 
                    context.BooksCategories
                    .Where(b=>b.Category.Name.ToLower() == genre)
                    .Select(b => b.Book.Title)
                    .ToList();

                finalLibrary.AddRange(books);
            }

            finalLibrary = finalLibrary
                .OrderBy(x => x)
                .ToList();

            return string.Join(Environment.NewLine, finalLibrary);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", null))
                .OrderByDescending(x=>x.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var b in books)
            {
                sb.AppendLine($"{b.Title} - {b.EditionType} - ${b.Price:F2}");
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var a in authors)
            {
                sb.AppendLine($"{a.FirstName} {a.LastName}");
            }

            return sb.ToString().TrimEnd();
                
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var authors = context.Books
              .Where(a => a.Author.LastName.ToLower().StartsWith(input.ToLower()))
              .OrderBy(x=>x.BookId)
              .Select(a => new
              {
                  a.Author.FirstName,
                  a.Author.LastName,
                  a.Title
              })
              
              .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var a in authors)
            {
                sb.AppendLine($"{a.Title} ({a.FirstName} {a.LastName})");
            }

            return sb.ToString().TrimEnd();

        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Select(b => b.BookId)
                .ToList();

            return books.Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var copies = context.Authors
                .Select(b => new
                {
                    FullName = $"{b.FirstName} {b.LastName}",
                    Books = b.Books.Select(b => b.Copies).Sum()
                })
                  .OrderByDescending(b => b.Books)
                  .Select(a => $"{a.FullName} - {a.Books}")
                .ToList();

            return   string.Join(Environment.NewLine, copies);
            
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var books = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Profit = c.CategoryBooks.Sum(b => b.Book.Copies * b.Book.Price)
                })
                .OrderByDescending(x => x.Profit).ThenBy(x => x.CategoryName)
                .Select(x => $"{x.CategoryName} ${x.Profit:f2}").ToList();

            return string.Join(Environment.NewLine, books);
             

        }

         public static string GetMostRecentBooks(BookShopContext context)
        {
            var books = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    MostRecentBooks = c.CategoryBooks
                    .OrderByDescending(rb => rb.Book.ReleaseDate)
                    .Select(rb => new
                    {
                        BookTitle = rb.Book.Title,
                        BookReleaseDate = rb.Book.ReleaseDate.Value.Year
                    }).Take(3)
                    
                }).OrderBy(c=>c.CategoryName);

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"--{book.CategoryName}");

                foreach (var b in book.MostRecentBooks)
                {
                    sb.AppendLine($"{b.BookTitle} ({b.BookReleaseDate})");
                }
            }

            return sb.ToString().TrimEnd();
        }

         public static void IncreasePrices(BookShopContext context)
        {
            List<Book> booksWhichPriceWillBeIncreased = context.Books
              .Where(b => b.ReleaseDate.Value.Year < 2005)
              .ToList();

            booksWhichPriceWillBeIncreased.ForEach(b => b.Price += 5);

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Copies < 4200)
                .ToList();

            int number = books.Count();

            context.Books.RemoveRange(books);
            context.SaveChanges();

            return number;
        }
    }
}
