using Core.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Data.Configuration
{
    public class MovieEntityConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.HasMany(m => m.Cinemas)
                .WithMany(c => c.Movies)
                .UsingEntity<CinemaMovie>(
                    cm => cm.HasOne(cm => cm.Cinema).WithMany(c => c.CinemaMovies).HasForeignKey(cm => cm.CinemaId),
                    cm => cm.HasOne(cm => cm.Movie).WithMany(c => c.CinemaMovies).HasForeignKey(cm => cm.MovieId),
                    cm =>
                    {
                        cm.HasKey(cm => new { cm.CinemaId, cm.MovieId });
                    }
                );
        }
    }
}
