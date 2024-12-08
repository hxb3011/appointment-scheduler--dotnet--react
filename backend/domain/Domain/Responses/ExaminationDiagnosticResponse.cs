using System.Text.Json.Serialization;

namespace AppointmentScheduler.Domain.Responses;

public class ExaminationDiagnosticResponse
{
    public string Name { get; set; }
    public double Price { get; set; }
    public uint Doctor { get; set; }
    [JsonPropertyName("diagnostic_service")]
    public uint DiagnosticService { get; set; }
    public uint Examination { get; set; }
}