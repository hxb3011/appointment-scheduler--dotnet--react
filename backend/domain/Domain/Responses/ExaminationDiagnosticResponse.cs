namespace AppointmentScheduler.Domain.Responses;

public class ExaminationDiagnosticResponse
{
    public string Name { get; set; }
    public double Price { get; set; }
    public uint DoctorId { get; set; }
    public uint DiagnosticServiceId { get; set; }
    public uint ExaminationId { get; set; }
}