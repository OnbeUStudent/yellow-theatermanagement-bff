using System;
using System.ComponentModel.DataAnnotations;
//test
namespace Dii_MovieCatalogSvc.Fake.Data
{
    public class Movie
    {
        public Guid MovieId { get; set; }

        public MovieMetadata MovieMetadata { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
