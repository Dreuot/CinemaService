using BackService.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackService.Data.Configuration
{
    public class CinemaEntityConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Address).HasMaxLength(500);
            builder.HasMany(c => c.Movies)
                .WithMany(m => m.Cinemas)
                .UsingEntity<CinemaMovie>(
                    cm => cm.HasOne(cm => cm.Movie).WithMany(c => c.CinemaMovies).HasForeignKey(cm => cm.MovieId),
                    cm => cm.HasOne(cm => cm.Cinema).WithMany(c => c.CinemaMovies).HasForeignKey(cm => cm.CinemaId),
                    cm =>
                    {
                        cm.HasKey(cm => new { cm.CinemaId, cm.MovieId });
                    }
                );
        }
    }
}
