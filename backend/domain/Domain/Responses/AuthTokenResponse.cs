using System.Text.Json.Serialization;

namespace AppointmentScheduler.Domain.Responses;

public class AuthTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "bearer";
}