namespace AppointmentScheduler.Domain.Entities;

public class SchedulerPart : BaseEntity
{
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
}