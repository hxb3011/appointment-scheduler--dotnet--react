namespace AppointmentScheduler.Domain.Business;

public interface IBehavioralEntity
{
    event EventHandler Created;
    event EventHandler Updated;
    event EventHandler Deleted;
    
    Task<bool> Create();
    Task<bool> Update();
    Task<bool> Delete();
}