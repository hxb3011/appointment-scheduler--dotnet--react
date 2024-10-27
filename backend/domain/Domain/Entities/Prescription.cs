namespace AppointmentScheduler.Domain.Entities;

public class Prescription : BaseEntity
{
    public uint ExaminationId { get; set; }
    public byte[] Document { get; set; }
    public string Description { get; set; }
}
