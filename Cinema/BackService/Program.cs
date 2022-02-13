using Core.Data;
using Core.Data.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
string connectionString = config["CinemaDb"] ?? "Filename=Cinema.db";

builder.Services.AddDbContext<CinemaContext>(options => options.UseSqlite(connectionString).LogTo(Console.WriteLine));

var app = builder.Build();

app.MapGet("/", (CinemaContext db) => {
    db.Database.EnsureCreated();

    return db.Cinemas.FirstOrDefault();
});

app.MapGet("/test", (CinemaContext db) => {
    db.Database.EnsureCreated();
    db.Add(new Cinema()
    {
        Name = "Люксор",
        Address = "Московское шоссе, барс"
    });

    db.SaveChanges();

    return Results.Redirect("/");
});

app.Run();
