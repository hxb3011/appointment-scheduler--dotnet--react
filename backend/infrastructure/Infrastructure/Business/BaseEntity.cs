using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.Infrastructure.Business;

internal abstract class BaseEntity : IBehavioralEntity, IRepositoryEntityInitializer
{
    protected IRepository _repository;
    protected DbContext _dbContext;
    private EventHandler _created, _updated, _deleted;

    event EventHandler IBehavioralEntity.Created
    {
        add => _created += value;
        remove => _created -= value;
    }

    event EventHandler IBehavioralEntity.Updated
    {
        add => _updated += value;
        remove => _updated -= value;
    }

    event EventHandler IBehavioralEntity.Deleted
    {
        add => _deleted += value;
        remove => _created -= value;
    }

    Task<bool> IBehavioralEntity.Create() => SaveChangesCUDWrap(Create, _created);
    Task<bool> IBehavioralEntity.Delete() => SaveChangesCUDWrap(Delete, _deleted);
    Task<bool> IBehavioralEntity.Update() => SaveChangesCUDWrap(Update, _updated);

    async Task<bool> IRepositoryEntityInitializer.Initialize(IRepository repository)
    {
        _repository = repository;
        _dbContext = await repository.GetService<DbContext>();
        return await Initilize();
    }

    private async Task<bool> SaveChangesCUDWrap(Func<Task<bool>> cud, EventHandler eh)
    {
        if (!await cud()) return false;
        if (await _dbContext.SaveChangesAsync() == 0) return false;
        eh?.Invoke(this, EventArgs.Empty);
        return true;
    }

    protected abstract Task<bool> Create();
    protected abstract Task<bool> Delete();
    protected virtual Task<bool> Initilize() => Task.FromResult(true);
    protected abstract Task<bool> Update();
}