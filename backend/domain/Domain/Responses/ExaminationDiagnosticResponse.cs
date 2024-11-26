namespace AppointmentScheduler.Domain.Responses;

public class ExaminationDiagnosticResponse
{
    public uint DoctorId { get; set; }
    public uint DiagnosticServiceId { get; set; }
    public uint ExaminationId { get; set; }
}