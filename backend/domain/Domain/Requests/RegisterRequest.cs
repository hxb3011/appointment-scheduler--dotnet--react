using System.Text.Json.Serialization;

namespace AppointmentScheduler.Domain.Requests;

public class RegisterRequest
{
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}