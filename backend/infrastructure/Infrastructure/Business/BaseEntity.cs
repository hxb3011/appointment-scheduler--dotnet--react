using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal abstract class BaseEntity : IBehavioralEntity, IRepositoryEntityInitializer
{
    protected IRepository _repository;
    protected DbContext _dbContext;

    Task<bool> IBehavioralEntity.Create() => SaveChangesCUDWrap(Create);
    Task<bool> IBehavioralEntity.Delete() => SaveChangesCUDWrap(Delete);
    Task<bool> IBehavioralEntity.Update() => SaveChangesCUDWrap(Update);

    async Task<bool> IRepositoryEntityInitializer.Initilize(IRepository repository)
    {
        _repository = repository;
        _dbContext = await repository.GetService<DbContext>();
        return await Initilize();
    }

    private async Task<bool> SaveChangesCUDWrap(Func<Task<bool>> cud)
        => await cud() && await _dbContext.SaveChangesAsync() != 0;

    protected abstract Task<bool> Create();
    protected abstract Task<bool> Delete();
    protected abstract Task<bool> Initilize();
    protected abstract Task<bool> Update();
}