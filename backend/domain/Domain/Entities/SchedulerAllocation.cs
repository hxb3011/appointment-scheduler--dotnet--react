namespace AppointmentScheduler.Domain.Entities;

public class SchedulerAllocation : BaseEntity
{
    public TimeOnly AtTime { get; set; }
}