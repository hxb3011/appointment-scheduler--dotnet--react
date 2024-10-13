namespace AppointmentScheduler.Domain.Entities;

public class Prescription
{
    public int Id { get; set; }
    public int ExaminationId { get; set; }
    public byte[] Document { get; set; }
    public string Description { get; set; }
}
