namespace AppointmentScheduler.Domain.Requests;

public class ExaminationDiagnosticRequest
{
    public uint Doctor { get; set; }
    public uint DiagnosticService { get; set; }
    public uint Examination { get; set; }
}