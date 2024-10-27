namespace AppointmentScheduler.Domain.Entities;

public class ExaminationService : BaseEntity
{
    public uint DoctorId { get; set; }
    public uint DiagnosticServiceId { get; set; }
    public uint ExaminationId { get; set; }
    public byte[] Document { get; set; }
}