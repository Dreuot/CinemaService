using BackService.AsyncConsumers;
using BackService.Data;
using BackService.Data.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
string connectionString = config["CinemaDb"] ?? "Filename=Cinema.db";

builder.Services.AddDbContext<CinemaContext>(options => options.UseSqlite(connectionString));
builder.Services.AddControllers();
builder.Services.AddHostedService<MessageBusSubscriber>();

var app = builder.Build();

app.UseCors(policy =>
{
    policy.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed(origin => true);
});

app.UseRouting();
app.MapControllers();

app.MapGet("/", (CinemaContext db) => {
    db.Database.EnsureCreated();

    return db.Cinemas.FirstOrDefault();
});

app.MapGet("/test", (CinemaContext db) => {
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    var cinema = new Cinema()
    {
        Name = "Люксор",
        Address = "Московское шоссе, барс"
    };

    db.Add(cinema);

    var movie = new Movie()
    {
        Name = "Шанчи",
        Description = "Да, очень интересное кино",
        Cinemas = new List<Cinema>()
    };

    movie.Cinemas.Add(cinema);

    db.Add(movie);

    db.SaveChanges();

    return Results.Redirect("/");
});

app.Run();
