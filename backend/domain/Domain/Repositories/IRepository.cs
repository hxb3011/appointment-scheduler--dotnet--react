using AppointmentScheduler.Domain.Business;

namespace AppointmentScheduler.Domain.Repositories;

public interface IRepository
{
    IEnumerable<TEntity> GetEntities<TEntity>() where TEntity : class, IBehavioralEntity;
    Task<TService> GetService<TService>();
    Task<TEntity> GetEntityBy<TKey, TEntity>(TKey key) where TEntity : class, IBehavioralEntity;
    Task<TEntity> ObtainEntity<TEntity>() where TEntity : class, IBehavioralEntity;
    bool TryGetKeyOf<TEntity, TKey>(TEntity entity, out TKey key) where TEntity : class, IBehavioralEntity;
}