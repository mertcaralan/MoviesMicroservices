using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Users.APP.Domain;

namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersDbController : ControllerBase
    {
        private readonly UsersDb _db;

        public UsersDbController(UsersDb db)
        {
            _db = db;
        }

        [HttpGet, Route("~/api/SeedDb")]
        public IActionResult Seed()
        {
            var dbFactory = new UsersDbFactory();

            dbFactory.SeedDb(_db);

            return Ok("Database seed successful.");
        }
    }
}
