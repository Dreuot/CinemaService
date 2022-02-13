using Core.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.Data
{
    public class CinemaContext : DbContext
    {
        public DbSet<Cinema> Cinemas { get; set; }

        public CinemaContext(DbContextOptions<CinemaContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //IConfiguration configuration = new ConfigurationBuilder()
            //    .AddEnvironmentVariables()
            //    .Build();

            //string connectionString = configuration["CinemaDb"] ?? "Filename=Cinema.db";

            //optionsBuilder
            //    .UseSqlite(connectionString)
            //    .LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CinemaContext).Assembly);
        }
    }
}
