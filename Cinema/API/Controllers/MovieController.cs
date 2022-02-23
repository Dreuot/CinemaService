using API.AsyncPublishers;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMessageBusPublisher _publisher;

        public MovieController(IMessageBusPublisher publisher)
        {
            _publisher = publisher;
        }

        [HttpGet]
        public async Task<IEnumerable<MovieViewModel>> GetAllMovies()
        {
            string response = await _publisher.GetAllMoviesAsync();
            IEnumerable<MovieViewModel> result;
            try
            {
                result = JsonSerializer.Deserialize<IEnumerable<MovieViewModel>>(response);

                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("{movieId}/cinema/{cinemaId}/seances")]
        public async Task<string> GetCinemaMovieSeances(int cinemaId, int movieId)
        {
            return await _publisher.GetCinemaMovieSeances(cinemaId, movieId);
        }
    }
}
