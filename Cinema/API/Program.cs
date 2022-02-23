using API.AsyncPublishers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IMessageBusPublisher, MessageBusPublisher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();
app.MapGet("/", async (IMessageBusPublisher publisher) =>
{
    string response = await publisher.GetAllMoviesAsync();

    return response;
});

app.Run();
