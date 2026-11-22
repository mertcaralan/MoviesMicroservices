using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Movies.APP.Domain;

namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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
