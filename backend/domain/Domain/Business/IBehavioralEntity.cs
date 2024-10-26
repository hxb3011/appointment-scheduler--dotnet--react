namespace AppointmentScheduler.Domain.Business;

public interface IBehavioralEntity
{
    Task<bool> Create();
    Task<bool> Update();
    Task<bool> Delete();
}