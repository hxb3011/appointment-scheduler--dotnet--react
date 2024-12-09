using System.Text.Json.Serialization;

namespace AppointmentScheduler.Domain.Responses;

public class DoctorRequest
{
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Position { get; set; }
    public string Certificate { get; set; }
    public uint RoleId { get; set; }
}