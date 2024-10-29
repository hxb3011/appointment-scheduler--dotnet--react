namespace AppointmentScheduler.Domain.Entities;

public class Appointment : BaseEntity
{
    public DateTime AtTime { get; set; }
    public uint State { get; set; }
    public uint? ProfileId { get; set; }
    public uint DoctorId { get; set; }
}