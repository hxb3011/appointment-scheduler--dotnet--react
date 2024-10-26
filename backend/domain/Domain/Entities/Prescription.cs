namespace AppointmentScheduler.Domain.Entities;

public class Prescription : BaseEntity
{
    public int ExaminationId { get; set; }
    public byte[] Document { get; set; }
    public string Description { get; set; }
}
