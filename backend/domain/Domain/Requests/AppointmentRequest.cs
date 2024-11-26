using System.Text.Json.Serialization;

namespace AppointmentScheduler.Domain.Requests;

public class AppointmentRequest
{
    public DateOnly Date { get; set;}
    [JsonPropertyName("begin_time")]
    public TimeOnly BeginTime { get; set; }
    [JsonPropertyName("end_time")]
    public TimeOnly EndTime { get; set; }
    public uint? Profile { get; set; }
    public uint Doctor { get; set; }
}