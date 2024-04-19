using APIService.Authorization;
using APIService.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserApiController : ControllerBase
{
    private readonly IAuth0UserManager _auth0UserManager;

    public UserApiController(IAuth0UserManager auth0UserManager)
    {
        _auth0UserManager = auth0UserManager;
    }

    [HttpGet("{userId}")]
    // [Authorize]
    public async Task<IActionResult> GetUser(string userId)
    {
        try
        {
            var authorizationToken = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationToken))
            {
                Unauthorized("Token not found");
            }
            var token = authorizationToken.ToString().Split(" ");
            var user = await _auth0UserManager.GetUserAsync(userId, token[1]);
            if (user == null)
                return BadRequest("Failed to get User");

            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Failed to retrieve user: {ex.Message}");
        }
    }

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
}