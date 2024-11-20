using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;

namespace AppointmentScheduler.Domain.Repositories;

public interface ISchedulerService
{
    TimeOnly FirstStart { get; set; }
    TimeOnly FirstEnd { get; set; }
    TimeOnly LastStart { get; set; }
    TimeOnly LastEnd { get; set; }
    TimeSpan BigStepGap { get; set; }
    TimeSpan StepGap { get; set; }
    IEnumerable<SchedulerPart> Parts { get; }
    IEnumerable<SchedulerAllocation> Allocations { get; }
    Task<SchedulerAllocation> Allocate(IDoctor doctor, DateOnly date, TimeOnly start, TimeOnly end);
    Task<SchedulerAllocation> Allocate(IDoctor doctor);
}