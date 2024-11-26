using System.Text.Json.Serialization;

namespace AppointmentScheduler.Domain.Responses;

public class PatientRequest
{
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}