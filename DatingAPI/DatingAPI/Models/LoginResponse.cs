using System.Text.Json.Serialization;

namespace DatingAPI.Models
{
    public class LoginResponse
    {
        [JsonPropertyName("user")]
        public User User { get; set; }
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
