using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIService.Authorization;

public class Auth0UserManager : IAuth0UserManager
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    IConfigurationSection auth0Settings;

    public Auth0UserManager(IHttpClientFactory httpClientFactory,
    IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;

        auth0Settings = configuration.GetSection("Auth0");
    }

    public async Task CreateUserAsync(UserInfo userInfo)
    {
        var auth0Settings = _configuration.GetSection("Auth0");

        using (var client = _httpClientFactory.CreateClient())
        {
            var auth0Url = $"https://{auth0Settings["Domain"]}/oauth/users";
            var content = new StringContent(JsonSerializer.Serialize(userInfo), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(auth0Url, content);

            response.EnsureSuccessStatusCode();
        }
    }

    public async Task<UserInfo> GetUserAsync(string userId, string token)
    {
        using (var client = _httpClientFactory.CreateClient("defaultToken"))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"{auth0Settings["Audience"]}users/{userId}");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserInfo>(json)!;
        }
    }


    // public async Task UpdateUserAsync(string userId, UserInfo updatedUserInfo)
    // {
    //     var json = JsonConvert.SerializeObject(updatedUserInfo);
    //     var content = new StringContent(json, Encoding.UTF8, "application/json");

    //     var response = await _httpClient.PatchAsync($"{BaseUrl}users/{userId}", content);

    //     response.EnsureSuccessStatusCode();
    // }

    // public async Task DeleteUserAsync(string userId)
    // {
    //     var response = await _httpClient.DeleteAsync($"{BaseUrl}users/{userId}");

    //     response.EnsureSuccessStatusCode();
    // }
}

public class UserInfo
{
    // Define user properties here
    [Required]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("nickname")]
    public string NickName { get; set; } = string.Empty;

    [JsonPropertyName("email_verified")]
    public bool isEmailVerified { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedOn { get; set; }
}

public interface IAuth0UserManager
{
    Task CreateUserAsync(UserInfo userInfo);

    Task<UserInfo> GetUserAsync(string userId, string token);
}
