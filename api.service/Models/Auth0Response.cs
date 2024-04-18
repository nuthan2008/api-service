namespace APIService.Models
{
    public class Auth0TokenResponse
    {
        public string access_token { get; set; } = String.Empty;
        public string token_type { get; set; } = String.Empty;
        public int expires_in { get; set; }
        // Add other properties as needed
    }
}