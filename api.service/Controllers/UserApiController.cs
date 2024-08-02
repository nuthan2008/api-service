using System.Text.Json;
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
        // try
        // {
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
        // }
        // catch (Exception ex)
        // {
        //     return StatusCode(500, $"Failed to retrieve user: {ex.Message}");
        // }
    }

    private List<User> users = new List<User>
        {
            new User { Id=1, FirstName="Nuthan", LastName="Gowda" },
            new User { Id=2, FirstName="Meena", LastName="Srinivas" },
            new User { Id=3, FirstName="Ganavi", LastName="Gowda" }
        };

    [HttpPost(Name = "CreateUser")]
    public async Task<IResult> CreateUser(UserInfo userInfo)
    {
        var result = await _auth0UserManager.CreateUserAsync(userInfo);

        // if (response.IsSuccessStatusCode)
        // {
        //     return response;
        // }
        // else
        // {
        //     var errorContent = await response.Content.ReadAsStringAsync();
        //     return new HttpResponseMessage(response.StatusCode)
        //     {
        //         Content = new StringContent(errorContent, Encoding.UTF8, "application/json")
        //     };
        // }
        return result.IsSuccessStatusCode ? Results.Ok(result) : await GenerateErrorResult(result.Content);
    }

    async Task<IResult> GenerateErrorResult(HttpContent errorContent)
    {
        var content = await errorContent.ReadAsStreamAsync();
        var errorModel = JsonSerializer.Deserialize<ErrorModel>(content);
        if (errorModel == null)
        {
            return Results.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Unexpected Error"
            );
        }

        return Results.Problem(
            statusCode: errorModel.StatusCode,
            title: errorModel.Error,
            extensions: new Dictionary<string, object?>
            {
                {"errors", new[] {errorModel.Message}}
            }
        );
    }
}