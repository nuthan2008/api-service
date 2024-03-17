using APIService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserApiController : ControllerBase
    {
        private List<User> users = new List<User>
        {
            new User { Id=1, FirstName="Nuthan", LastName="Gowda" },
            new User { Id=2, FirstName="Meena", LastName="Srinivas" },
            new User { Id=3, FirstName="Ganavi", LastName="Gowda" }
        };

        [HttpGet(Name = "GetUsers")]
        public IActionResult Get()
        {
            return Ok(users);
        }


        [HttpGet("{id}", Name = "GetUserById")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            var user = users.Find(f => f.Id == id);

            if (user != null)
            {
                return Ok(user); // 200 OK
            }

            return NotFound(); // 404 Not Found
        }
    }
}