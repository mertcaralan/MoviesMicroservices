using Microsoft.AspNetCore.Mvc;
using Movies.APP.Domain;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesDbController : ControllerBase
    {
        [HttpGet, Route("~/api/SeedDb")]
        public IActionResult Seed()
        {
            var dbFactory = new MoviesDbFactory();
            dbFactory.SeedDb();
            return Ok("Database seed successful.");
        }
    }
}
