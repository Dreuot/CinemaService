using BackService.AsyncConsumers;
using BackService.DTO;
using BackService.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace BackService.EventProcessings
{
    public class GetAllMoviesProcessor : IAsyncEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public GetAllMoviesProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public string ProcessEvent(AsyncMessage message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetService<CinemaContext>();
                var movies = _db.Movies.Include(m => m.Cinemas).ToList();
                var moviesDto = movies.Select(m => new MovieDTO()
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Cinemas = m.Cinemas.ToList().Select(c => new MovieCinemaDTO()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Address = c.Address
                    })
                });

                return JsonSerializer.Serialize(moviesDto, new JsonSerializerOptions()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
                });
            }
        }
    }
}
