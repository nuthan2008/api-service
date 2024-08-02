using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using APIService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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

    private async Task<string> GetAccessToken()
    {
        using (var client = _httpClientFactory.CreateClient())
        {
            // var requestBody1 = new StringContent(
            //     $"{{\"client_id\":\"{clientId}\",\"client_secret\":\"{clientSecret}\",\"audience\":\"https://{domain}/api/v2/\",\"grant_type\":\"client_credentials\"}}",
            //     Encoding.UTF8,
            //     "application/json");

            var requestBody = new
            {
                grant_type = "client_credentials",
                audience = auth0Settings["Audience"],
                client_id = auth0Settings["ClientId"],
                client_secret = auth0Settings["ClientSecret"]
            };
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = client.PostAsync($"https://{auth0Settings["Domain"]}/oauth/token", content).Result;

            if (response.IsSuccessStatusCode)
            {
                // var tokenResponse = await response.Content.ReadAsStringAsync().Result;
                // dynamic tokenObject = JsonSerializer.DeserializeAsync(tokenResponse);
                var responseData = await JsonSerializer.DeserializeAsync<Auth0TokenResponse>(await response.Content.ReadAsStreamAsync());
                return responseData?.access_token;
            }
            else
            {
                throw new Exception("Failed to retrieve access token.");
            }
        }
    }

    public async Task<HttpResponseMessage> CreateUserAsync(UserInfo userInfo)
    {
        var auth0Settings = _configuration.GetSection("Auth0");

        using (var client = _httpClientFactory.CreateClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessToken());
            var auth0Url = $"{auth0Settings["Audience"]}users";

            userInfo.AppMetadata.SysCreatedDate = DateTime.UtcNow;
            userInfo.AppMetadata.SysModDate = DateTime.UtcNow;
            userInfo.NickName = $"{userInfo.FirstName} {userInfo.LastName}";
            var jsonOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var content = new StringContent(JsonSerializer.Serialize(userInfo, jsonOptions), Encoding.UTF8, "application/json");

            return await client.PostAsync(auth0Url, content);
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
    [JsonPropertyName("user_id")]
    public string Id { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("given_name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("family_name")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("nickname")]
    public string NickName { get; set; } = string.Empty;

    [JsonPropertyName("email_verified")]
    public bool isEmailVerified { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("connection")]
    public string Connection { get; set; } = "Username-Password-Authentication";

    [JsonPropertyName("user_metadata")]
    public required UserMetadata UserMetadata { get; set; }

    [JsonPropertyName("app_metadata")]
    public required AppMetadata AppMetadata { get; set; }
}

public class UserMetadata
{
    [JsonPropertyName("contact")]
    public string Contact { get; set; } = string.Empty;

    [JsonPropertyName("publicName")]
    public string PublicName { get; set; } = string.Empty;

    [JsonPropertyName("plan")]
    public string plan { get; set; } = string.Empty;
}

public class AppMetadata
{
    [JsonPropertyName("roles")]
    public string Roles { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("userGroup")]
    public string UserGroup { get; set; } = string.Empty;

    [JsonPropertyName("sysDomainType")]
    public string SysDomainType { get; set; } = string.Empty;

    [JsonPropertyName("sysTenant")]
    public string SysTenant { get; set; } = string.Empty;

    [JsonPropertyName("sysCreatedBy")]
    public string SysCreatedBy { get; set; } = string.Empty;

    [JsonPropertyName("sysCreatedDate")]
    public DateTime SysCreatedDate { get; set; }

    [JsonPropertyName("sysModBy")]
    public string SysModBy { get; set; } = string.Empty;

    [JsonPropertyName("sysModDate")]
    public DateTime SysModDate { get; set; }

    [JsonPropertyName("sysLocale")]
    public string SysLocale { get; set; } = string.Empty;
}

public interface IAuth0UserManager
{
    Task<HttpResponseMessage> CreateUserAsync(UserInfo userInfo);

    Task<UserInfo> GetUserAsync(string userId, string token);
}