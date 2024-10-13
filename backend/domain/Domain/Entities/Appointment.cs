namespace AppointmentScheduler.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }
    public TimeOnly AtTime { get; set; }
    public int State { get; set; }
    public int ProfileId { get; set; }
}