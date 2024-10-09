using System.Text.Json.Serialization;

namespace TinyUrl.GenerationService.Infrastructure.Contracts.Responses
{
    public class UserContract
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("username")]
        public string? Username { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }
}
