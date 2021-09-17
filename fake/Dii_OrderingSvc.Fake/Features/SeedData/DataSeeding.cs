using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using DiiCommon.Time;
using Dii_OrderingSvc.Fake.Data;

namespace Dii_OrderingSvc.Fake.Features.SeedData
{
    public static class DataSeeding
    {
        /// <summary>
        /// Given the dot-deliminited name of a subdirectory of "DiiLegacy.Data" (e.g. "Assets.MovieMetadata")
        /// return a list of the JSON strings for each embedded resource in that subdirectory.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetJsonAssets(string directory) // e.g. "Assets.MovieMetadata"
        {
            string manifestModule = typeof(DataSeeding).Assembly.ManifestModule.ToString();      // "DiiLegacy.Data.Assets.dll"
            string root = Path.GetFileNameWithoutExtension(manifestModule);                      // "DiiLegacy.Data"
            string matchLeft = $"{root}.{directory}.";                              // e.g. "DiiLegacy.Data.MovieMetadata."

            var manifestResourceNames = typeof(DataSeeding).Assembly.GetManifestResourceNames(); // e.g. "DiiLegacy.Data.Assets.MovieMetadata.tt1520211.json", ...
            foreach (string name in manifestResourceNames.Where(n => n.StartsWith(matchLeft)))
            {
                using Stream stream = typeof(DataSeeding).Assembly.GetManifestResourceStream(name);
                using StreamReader sr = new StreamReader(stream);
                string content = sr.ReadToEnd();
                yield return content;
            }
        }

        public static void SeedData(OrderingSvcContext context)
        {
            const string theatercode_boulevard = "boulevard";
            const string theatercode_mchenry = "mchenry";

            DetailedTheater[] detailedTheaters = {
                new DetailedTheater()
                {
                    TheaterCode = theatercode_boulevard,
                    TheaterName = "Boulevard Drive-In Theater",
                    AddressLine1 = "1051 Merriam Ln",
                    City = "Kansas City",
                    State = "KS",
                    Zip = "66103",
                    ThemeType = ThemeType.Cerulean
                },
                new DetailedTheater()
                {
                    TheaterCode = theatercode_mchenry,
                    TheaterName = "McHenry Outdoor Theater",
                    AddressLine1 = "1510 N Chapel Hill Rd",
                    City = "McHenry",
                    State = "IL",
                    Zip = "60051",
                    ThemeType = ThemeType.Journal
                }
            };

            foreach (var detailedTheater in detailedTheaters)
            {
                if (!context.DetailedTheaters.Any(d => d.TheaterCode == detailedTheater.TheaterCode))
                {
                    context.Add(detailedTheater);
                }
            }
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                string str = ex.ToString();
            }

            foreach (string json in GetJsonAssets("Assets.MovieMetadata"))
            {
                var movieMetadata = MovieMetadata.FromJson(json);
                if (!context.MovieMetadatas.Any(m => m.ImdbId == movieMetadata.ImdbId))
                {
                    var movie = new Movie
                    {
                        Title = movieMetadata.Title,
                        MovieMetadata = movieMetadata
                    };
                    context.Add(movie);
                    context.Add(movie.MovieMetadata);
                }
            }

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                string str = ex.ToString();
            }

            List<Booking> bookings = new List<Booking>();
            var movies = context.Movies.OrderBy(m => m.MovieMetadata.Year).ToList(); // Movies in order by year
            var months = new MonthsStartingAt(DateTime.Now, 4);
            for (int i = 0; i < months.Count && i < movies.Count; i++)
            {
                // Book boulevard with the movies in chronological order
                var boulevardBooking = new Booking()
                {
                    TheaterCode = theatercode_boulevard,
                    MonthId = months[i].MonthId,
                    MovieId = movies[i].MovieId
                };
                if (!context.Bookings.Any(b => b.MonthId == boulevardBooking.MonthId && b.TheaterCode == theatercode_boulevard))
                {
                    context.Add(boulevardBooking);
                }

                // Book mchenry with the movies in reverse chronological order
                var mchenryBooking = new Booking()
                {
                    TheaterCode = theatercode_mchenry,
                    MonthId = months[i].MonthId,
                    MovieId = movies[movies.Count - 1 - i].MovieId
                };
                if (!context.Bookings.Any(b => b.MonthId == mchenryBooking.MonthId && b.TheaterCode == theatercode_mchenry))
                {
                    context.Add(mchenryBooking);
                }
            }
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                string str = ex.ToString();
            }
        }
    }
}
