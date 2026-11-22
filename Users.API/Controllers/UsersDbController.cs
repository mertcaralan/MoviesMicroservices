using Microsoft.AspNetCore.Mvc;
using Users.APP.Domain;

namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersDbController : ControllerBase
    {
        [HttpGet, Route("~/api/SeedDb")]
        public IActionResult Seed()
        {
            var dbFactory = new UsersDbFactory();

            dbFactory.SeedDb();

            return Ok("Database seed successful.");
        }
    }
}
