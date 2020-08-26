namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {

        public const string ErrorMessage = "Invalid Data";
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesDtos = JsonConvert.DeserializeObject<ImportGameDTO[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            var games = new List<Game>();
            var developers = new List<Developer>();
            var genres = new List<Genre>();
            var tags = new List<Tag>();

            foreach (var gameDto in gamesDtos)
            {


                if (!IsValid(gameDto) || gameDto.Tags.Length == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                DateTime releasedOn;

                bool isValidReleaseDate = DateTime.TryParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out releasedOn);

                int tagsCount = 0;
                Genre genre;
                Developer dev;

                dev = developers.FirstOrDefault(x => x.Name == gameDto.Developer);
                genre = genres.FirstOrDefault(x => x.Name == gameDto.Genre);



                var game = new Game()
                {
                    Name = gameDto.Name,
                    ReleaseDate = releasedOn,
                    Price = gameDto.Price,
                    Developer = dev == null ? new Developer() { Name = gameDto.Developer } : dev,
                    Genre = genre == null ? new Genre() {  Name=gameDto.Genre} :genre

                };

                foreach (var tag in gameDto.Tags)
                {
                    if (tags.FirstOrDefault(t => t.Name == tag) == null)
                    {
                        var tagtoAdd = new Tag() { Name = tag };
                        game.GameTags.Add(new GameTag { Game = game, Tag = tagtoAdd });
                        tags.Add(tagtoAdd);
                    }
                    else
                    {
                        var foundTag = tags.FirstOrDefault(t => t.Name == tag);
                        game.GameTags.Add(new GameTag { Game = game, Tag = foundTag });
                    }

                    tagsCount++;
                }

                games.Add(game);

                if (dev == null)
                {
                    developers.Add(game.Developer);
                }
                if (genre == null)
                {
                    genres.Add(game.Genre);
                }

                sb.AppendLine(string.Format($"Added {game.Name} ({game.Genre.Name}) with {tagsCount} tags"));
            }

            context.Tags.AddRange(tags);
            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {

            var usersDtos = JsonConvert.DeserializeObject<ImportUsersAndCardsDTO[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<User> users = new List<User>();

            foreach (var userDto in usersDtos)
            {
                if (!IsValid(userDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var user = new User()
                {
                    FullName = userDto.FullName,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Age = userDto.Age
                };

                foreach (var cardDto in userDto.Cards)
                {
                    if (!IsValid(cardDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var card = new Card()
                    {
                        Number = cardDto.Number,
                        Cvc = cardDto.Cvc,
                        Type = (CardType)cardDto.Type
                    };


                    user.Cards.Add(card);
                }
                users.Add(user);


                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
            }

            context.Users.AddRange(users);
            context.SaveChanges();


            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportPurchaseDTO[]), new XmlRootAttribute("Purchases"));

            var purchasesDto = (ImportPurchaseDTO[])xmlSerializer.Deserialize(new StringReader(xmlString));

            StringBuilder sb = new StringBuilder();

            var purchases = new List<Purchase>();

            foreach (var purchaseDto in purchasesDto)
            {
                if (!IsValid(purchaseDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime date;

                bool isValiDate = DateTime.TryParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out date);

                PurchaseType parsedType;

                var tryParseType = Enum.TryParse(purchaseDto.Type, out parsedType);

                if (!isValiDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                };


                var card = context.Cards.FirstOrDefault(x => x.Number == purchaseDto.Card);

                var game = context.Games.FirstOrDefault(x => x.Name == purchaseDto.title);

                var purchase = new Purchase()
                {
                    Type = parsedType,
                    ProductKey = purchaseDto.Key,
                    Date = date,
                    Card = card,
                    Game = game

                };
                var username = context.Users.FirstOrDefault(x => x.Id == purchase.Card.UserId);
                purchases.Add(purchase);
                sb.AppendLine($"Imported {purchaseDto.title} for {username.Username}");
            }

            context.Purchases.AddRange(purchases);
            // Save Changes
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}