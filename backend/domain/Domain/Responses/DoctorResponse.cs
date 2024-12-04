using System.Text.Json.Serialization;

namespace AppointmentScheduler.Domain.Responses;

public class DoctorResponse
{
    public uint Id { get; set; }
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
    [JsonPropertyName("username")]
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Position { get; set; }
    public string Certificate { get; set; }
    public uint Role { get; set; }
}