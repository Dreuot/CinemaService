using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public class CinemaContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            string connectionString = configuration["CinemaDb"] ?? "Filename=Cinema.db";

            optionsBuilder
                .UseSqlite(connectionString)
                .LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
