using BackService.Data;
using BackService.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaController : ControllerBase
    {
        private CinemaContext _db;

        public CinemaController(CinemaContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IEnumerable<Cinema> GetCinemas()
        {
            return _db.Cinemas.ToList();
        }

        [HttpGet("{id}")]
        public Cinema GetCinema(int id)
        {
            return _db.Cinemas.FirstOrDefault(c => c.Id == id);
        }

        [HttpPost]
        public void CreateCinema(Cinema cinema)
        {
            _db.Add(cinema);
            _db.SaveChanges();
        }

        [HttpPut("{id}")]
        public void UpdateCinema(Cinema cinema)
        {
            _db.Attach(cinema);
            _db.Entry(cinema).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}
