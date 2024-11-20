
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace AppointmentScheduler.Infrastructure.Repositories;

internal class ResourceManagerServiceImpl : IResourceManagerService
{
    Stream IResourceManagerService.Resource<TEntity>(string resourceId, bool readOnly)
        => new FileStream(
            Path.Combine("files", typeof(TEntity).FullName + resourceId),
            readOnly ? FileMode.Open : FileMode.Create,
            readOnly ? FileAccess.Read : FileAccess.Write
        );
}