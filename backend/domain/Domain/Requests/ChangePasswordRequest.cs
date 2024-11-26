using System.Text.Json.Serialization;

namespace AppointmentScheduler.Domain.Responses;

public class ChangePasswordRequest
{
    [JsonPropertyName("old")]
    public string OldPassword { get; set; }
    [JsonPropertyName("new")]
    public string NewPassword { get; set; }
}