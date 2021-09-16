using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Dii_OrderingSvc.Fake.Data
{
    public class OrderingSvcContext : DbContext
    {
        public OrderingSvcContext(DbContextOptions<OrderingSvcContext> options)
            : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<DetailedTheater> DetailedTheaters { get; set; }
        public DbSet<MovieMetadata> MovieMetadatas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<SnackPackOrder> SnackPackOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Booking
            builder.Entity<Booking>(booking =>
            {
                booking.HasKey(booking => new { booking.TheaterCode, booking.MonthId });
                booking.HasOne(booking => booking.Movie)
                    .WithMany(movie => movie.Bookings)
                    .HasForeignKey(booking => booking.MovieId);
                booking.HasOne(booking => booking.DetailedTheater)
                    .WithMany(theater => theater.Bookings)
                    .HasForeignKey(booking => booking.TheaterCode);
            });

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

            // DetailedTheater
            builder.Entity<DetailedTheater>(detailedTheater =>
            {
                int maxLengthForThemeType = Enum.GetNames(typeof(ThemeType)).Max(arr => arr.Length);

                detailedTheater.Property(e => e.ThemeType)
                    .HasMaxLength(maxLengthForThemeType)
                    .HasConversion(x => x.ToString(), x => (ThemeType)Enum.Parse(typeof(ThemeType), x)); // Persist as string
            });

            // SnackPackOrder
            builder.Entity<SnackPackOrder>(sno =>
            {
                sno.HasKey(sno => new { sno.TheaterCode, sno.MonthId });
                sno.HasOne(sno => sno.DetailedTheater)
                    .WithMany(theater => theater.SnackPackOrders)
                    .HasForeignKey(sno => sno.TheaterCode);
            });
        }
    }
}
