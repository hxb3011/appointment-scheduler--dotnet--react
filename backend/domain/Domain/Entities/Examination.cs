namespace AppointmentScheduler.Domain.Entities;

public class Examination : BaseEntity
{
    public uint DoctorId { get; set; }
    public uint AppointmentId { get; set; }
    public string Diagnostic { get; set; }
    public string Description { get; set; }
    public uint State { get; set; }
}