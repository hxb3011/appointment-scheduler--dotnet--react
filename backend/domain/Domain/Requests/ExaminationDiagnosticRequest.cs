namespace AppointmentScheduler.Domain.Requests;

public class ExaminationDiagnosticRequest
{
    public uint DoctorId { get; set; }
    public uint DiagnosticServiceId { get; set; }
    public uint ExaminationId { get; set; }
}