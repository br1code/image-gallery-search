using System.Text.Json.Serialization;

namespace ImageGallerySearch.WebApi.Models.External
{
    public class AuthenticationResult
    {
        [JsonPropertyName("auth")]
        public bool Auth { get; set; }
        
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}