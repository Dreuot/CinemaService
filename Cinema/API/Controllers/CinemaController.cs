using API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class CinemaController : ControllerBase
    {
        private readonly ILogger<CinemaController> _logger;

        public CinemaController(ILogger<CinemaController> logger)
        {
            _logger = logger;
        }

        [HttpGet("[action]")]
        public IEnumerable<MovieViewModel> GetMovies()
        {
            return null;
        }
    }
}