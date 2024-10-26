namespace AppointmentScheduler.Domain.Entities;

public class ExaminationService : BaseEntity
{
    public int DoctorId { get; set; }
    public int DiagnosticServiceId { get; set; }
    public int ExaminationId { get; set; }
    public byte[] Document { get; set; }
}