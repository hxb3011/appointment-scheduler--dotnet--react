using System.Text.Json.Serialization;

namespace AppointmentScheduler.Domain.Responses;

public class ProfileResponse
{
    public uint Id { get; set; }
    public uint Patient { get; set; }
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
    [JsonPropertyName("birthdate")]
    public DateOnly DateOfBirth { get; set; }
    public char Gender { get; set; }
}