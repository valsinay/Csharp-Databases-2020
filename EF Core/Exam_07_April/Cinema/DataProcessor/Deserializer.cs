namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Cinema.Data;
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var moviesDto = JsonConvert.DeserializeObject<ImportMovieDTO[]>(jsonString);

            var movies = new List<Movie>();

            StringBuilder sb = new StringBuilder();

            foreach (var movieDto in moviesDto)
            {
                if (!IsValid(movieDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (movies.Any(m=> m.Title == movieDto.Title))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }


                //var tryParseGenre = Enum.TryParse(typeof(Genre), movieDto.Genre, out object parsedGenre);


                Movie movie = new Movie()
                {
                    Title = movieDto.Title,
                    Genre = movieDto.Genre,
                    Duration = movieDto.Duration,
                    Rating = movieDto.Rating,
                    Director = movieDto.Director
                };

                movies.Add(movie);
                sb.AppendLine(String.Format(SuccessfulImportMovie, movieDto.Title, movieDto.Genre, $"{movieDto.Rating:f2}"));
            }

            context.Movies.AddRange(movies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
            

        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            //    var hallsSeatsDtos= JsonConvert.DeserializeObject<ImportHallsAndSeatsDTO[]>(jsonString);

            //    var hallsAndSeats = new List<Hall>();
            //    var seats = new List<Seat>();

            //    StringBuilder sb = new StringBuilder();

            //    foreach (var hallDto in hallsSeatsDtos)
            //    {
            //        if (!IsValid(hallDto))
            //        {
            //            sb.AppendLine(ErrorMessage);
            //            continue;
            //        }

            //        string status = "";

            //        if(hallDto.Is4Dx )
            //        {
            //            status = hallDto.Is3D ? "4Dx/3D" : "4Dx";
            //        }
            //        else if (hallDto.Is3D)
            //        {
            //            status = "3D";
            //        }
            //        else
            //        {
            //            status = "Normal";
            //        }

            //       var hall  = new Hall()
            //        {
            //            Name = hallDto.Name,
            //            Is4Dx = hallDto.Is4Dx,
            //            Is3D = hallDto.Is3D
            //        };

            //        context.Halls.Add(hall);


            //        AddSeatsInDatabase(context, hall.Id, hallDto.Seats);

            //        sb.AppendLine(String.Format(SuccessfulImportHallSeat, hallDto.Name, status, hallDto.Seats));
            //    }

            //    context.SaveChanges();
            //    return sb.ToString().TrimEnd(); 
            var hallSeatDtos = JsonConvert.DeserializeObject<ImportHallsAndSeatsDTO[]>(jsonString);
            var halls = new List<Hall>();

            var sb = new StringBuilder();

            foreach (var hallSeatDto in hallSeatDtos)
            {
                if (!IsValid(hallSeatDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var hall = new Hall
                {
                    Name = hallSeatDto.Name,
                    Is3D = hallSeatDto.Is3D,
                    Is4Dx = hallSeatDto.Is4Dx,
                };

                for (int i = 0; i < hallSeatDto.Seats; i++)
                {
                    hall.Seats.Add(new Seat());
                }

                halls.Add(hall);

                string status = "";

                if (hall.Is4Dx)
                {
                    status = hall.Is3D ? "4Dx/3D" : "4Dx";
                }
                else if (hall.Is3D)
                {
                    status = "3D";
                }
                else
                {
                    status = "Normal";
                }

                sb.AppendLine(string.Format(SuccessfulImportHallSeat, hall.Name, status, hall.Seats.Count));
            }

            context.Halls.AddRange(halls);
            context.SaveChanges();

            return sb.ToString();
        }

        private static void AddSeatsInDatabase(CinemaContext context, int hallId, int seatCount)
        {
            var seats = new List<Seat>();

            for (int i = 0; i < seatCount; i++)
            {
                seats.Add(new Seat
                {
                    HallId = hallId
                });
            }
            context.AddRange(seats);
            context.SaveChanges();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            throw new NotImplementedException();
        }
        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}