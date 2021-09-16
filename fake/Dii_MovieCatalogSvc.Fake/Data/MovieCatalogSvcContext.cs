using Microsoft.EntityFrameworkCore;

namespace Dii_MovieCatalogSvc.Fake.Data
{
    public class MovieCatalogSvcContext : DbContext
    {
        public MovieCatalogSvcContext(DbContextOptions<MovieCatalogSvcContext> options)
            : base(options)
        {
        }

        public DbSet<MovieMetadata> MovieMetadatas { get; set; }
        public DbSet<Movie> Movie { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Movie and MovieMetadata use table splitting to share a table.
            // (See https://docs.microsoft.com/en-us/ef/core/modeling/table-splitting)
            builder.Entity<Movie>(movie =>
            {
                movie.HasOne(t => t.MovieMetadata).WithOne()
                    .HasForeignKey<MovieMetadata>(movieMetadata => movieMetadata.MovieMetadataId);

                // Anything shared between the contexts should have the same table/column names
                movie.ToTable("Movies");
                movie.Property(o => o.Title).HasColumnName("Title");
            });
            builder.Entity<MovieMetadata>(movieMetadata =>
            {
                // Anything shared between the contexts should have the same table/column names
                movieMetadata.ToTable("Movies");
                movieMetadata.Property(o => o.Title).HasColumnName("Title");
            });
        }
    }
}
