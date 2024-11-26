using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace AppointmentScheduler.Domain.Responses;

public class AppointmentResponse
{
    public uint Id { get; set; }
    [JsonPropertyName("at")]
    public DateTime AtTime { get; set; }
    public uint Number { get; set; }
    public uint State { get; set; }
    public uint? Profile { get; set; }
    public uint Doctor { get; set; }
}