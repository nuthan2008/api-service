namespace SampleDotNetCoreApiProject.Models
{
    public class Auth0TokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        // Add other properties as needed
    }
}