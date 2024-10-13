namespace AppointmentScheduler.Domain.Entities;

public interface IRepository
{
    IEnumerable<TEntity> GetEntities<TEntity>() where TEntity : IBehavioralEntity;
    TService GetService<TService>();
    TEntity ObtainEntity<TEntity>() where TEntity : IBehavioralEntity;
    bool TryGetEntityBy<TKey, TEntity>(TKey? key, out TEntity entity) where TEntity : IBehavioralEntity;
    bool TryGetKeyOf<TEntity, TKey>(TEntity? entity, out TKey key) where TEntity : IBehavioralEntity;
}