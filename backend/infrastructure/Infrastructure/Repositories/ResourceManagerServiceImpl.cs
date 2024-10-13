
using AppointmentScheduler.Domain.Entities;

namespace AppointmentScheduler.Infrastructure.Repositories;

internal class ResourceManagerServiceImpl : IResourceManagerService
{
    Stream IResourceManagerService.LoadResource<TEntity>(string resourceId)
    {
        throw new NotImplementedException();
    }

    bool IResourceManagerService.StoreResource<TEntity>(string resourceId, Stream resourceStream)
    {
        throw new NotImplementedException();
    }
}