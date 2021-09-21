using Dii_MovieCatalogSvc.Fake.Data;
using System; 
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dii_MovieCatalogSvc.Fake.Features.SeedData
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

        public static void SeedData(MovieCatalogSvcContext context)
        {
            long longMovieId = 0;
            foreach (string json in GetJsonAssets("Assets.MovieMetadata"))
            {
                var movieMetadata = MovieMetadata.FromJson(json);
                if (!context.MovieMetadatas.Any(m => m.ImdbId == movieMetadata.ImdbId))
                {
                    longMovieId++;
                    byte[] guidData = new byte[16];
                    Array.Copy(BitConverter.GetBytes(longMovieId), guidData, 8);
                    var guidMovieId = new Guid(guidData);
                    var movie = new Movie
                    {
                        MovieId = guidMovieId,
                        Title = movieMetadata.Title,
                        MovieMetadata = movieMetadata
                    };
                    context.Add(movie);
                    context.Add(movie.MovieMetadata);
                }
            }

            context.SaveChanges();
        }
    }
}
