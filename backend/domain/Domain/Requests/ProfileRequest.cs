using System.Text.Json.Serialization;

namespace AppointmentScheduler.Domain.Requests;

public class ProfileRequest
{
    public uint Patient { get; set; }
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
    [JsonPropertyName("date_of_birth")]
    public DateOnly DateOfBirth { get; set; }
    public char Gender { get; set; }
}