namespace AppointmentScheduler.Domain.Entities;

public class Appointment : BaseEntity
{
    public TimeOnly AtTime { get; set; }
    public int State { get; set; }
    public int ProfileId { get; set; }
}