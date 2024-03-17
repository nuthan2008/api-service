using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using APIService.Models;

namespace APIService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory
        , IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await GetAuthTokenFromAuth0(loginRequest);

            if (!string.IsNullOrEmpty(token))
            {
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        private async Task<string> GetAuthTokenFromAuth0(LoginRequest loginRequest)
        {
            var auth0Settings = _configuration.GetSection("Auth0");

            using (var client = _httpClientFactory.CreateClient())
            {
                // Auth0 Authentication API endpoint
                var auth0Url = $"https://{auth0Settings["Domain"]}/oauth/token";

                // Prepare request parameters
                var requestBody = new
                {
                    grant_type = auth0Settings["grantType"],
                    username = loginRequest.Username,
                    password = loginRequest.Password,
                    audience = auth0Settings["Audience"],
                    client_id = auth0Settings["ClientId"],
                    client_secret = auth0Settings["ClientSecret"],
                    realm = auth0Settings["realm"]
                };

                // var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                // Send request to Auth0
                var response = await client.PostAsync(auth0Url, content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Parse and return the access token
                    var responseData = await JsonSerializer.DeserializeAsync<Auth0TokenResponse>(await response.Content.ReadAsStreamAsync());
                    return responseData?.access_token;
                }

                // Authentication failed
                return null;
            }
        }
    }
}