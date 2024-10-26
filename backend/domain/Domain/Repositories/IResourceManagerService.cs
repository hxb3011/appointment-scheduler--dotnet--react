using AppointmentScheduler.Domain.Business;

namespace AppointmentScheduler.Domain.Repositories;

public interface IResourceManagerService
{
    Stream LoadResource<TEntity>(string resourceId) where TEntity : IBehavioralEntity;
    bool StoreResource<TEntity>(string resourceId, Stream resourceStream) where TEntity : IBehavioralEntity;
}