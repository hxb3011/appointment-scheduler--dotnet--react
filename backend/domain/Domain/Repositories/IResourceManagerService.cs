using AppointmentScheduler.Domain.Business;

namespace AppointmentScheduler.Domain.Repositories;

public interface IResourceManagerService
{
    Stream Resource<TEntity>(string resourceId, bool readOnly = true) where TEntity : IBehavioralEntity;
}