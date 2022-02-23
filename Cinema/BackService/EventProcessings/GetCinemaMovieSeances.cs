using BackService.AsyncConsumers;
using BackService.Data;
using BackService.DTO;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace BackService.EventProcessings
{
    public class GetCinemaMovieSeances : IAsyncEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public GetCinemaMovieSeances(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public string ProcessEvent(AsyncMessage message)
        {
            BackService.EventProcessings.Params.GetCinemaMovieSeances @params = JsonSerializer.Deserialize<BackService.EventProcessings.Params.GetCinemaMovieSeances>(message.Body);

            using (var scope = _scopeFactory.CreateScope())
            {
                var _db = scope.ServiceProvider.GetService<CinemaContext>();
                var cinemaQuery = _db.Cinemas
                    .Where(c => c.Id == @params.CinemaId)
                    .Include(c => c.Movies.Where(m => m.Id == @params.MovieId))
                    .ThenInclude(m => m.Seances.Where(s => s.Hall.CinemaId == @params.CinemaId))
                    .ThenInclude(s => s.Hall)
                    .ThenInclude(h => h.Places);

                var sql = cinemaQuery.ToQueryString();
                Console.WriteLine(sql);

                var cinema = cinemaQuery.FirstOrDefault();

                string result = null;

                if (cinema != null)
                {
                    CinemaDTO cinemaDTO = new CinemaDTO()
                    {
                        Id = cinema.Id,
                        Name = cinema.Name,
                        Address = cinema.Address,
                        Seances = cinema.Movies
                            .FirstOrDefault()
                            ?.Seances
                            .Select(s => new SeanceDTO()
                            {
                                Id = s.Id,
                                BeginTime = s.BeginTime,
                                Duration = s.Duration,
                                Hall = s.Hall.Name,
                                Places = s.Hall.Places.Select(p => new PlaceDTO()
                                {
                                    Row = p.Row,
                                    Number = p.Number,
                                    Reserved = false
                                })
                            })
                    };

                    result = JsonSerializer.Serialize(cinemaDTO, new JsonSerializerOptions()
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
                    });
                }

                return result;
            }
        }
    }
}
