using System.Linq.Expressions;
using AppointmentScheduler.Domain.Business;

namespace AppointmentScheduler.Domain.Repositories;

public interface IRepository : IDisposable
{
    IEnumerable<TEntity> GetEntities<TEntity>(
        int skip = 0, int take = 20, string orderByProperty = null, bool descending = false,
        string whereProperty = null, object andValue = null, bool areEqual = true
    ) where TEntity : class, IBehavioralEntity;
    Task<TService> GetService<TService>();
    Task<TEntity> GetEntityBy<TKey, TEntity>(TKey key) where TEntity : class, IBehavioralEntity;
    Task<TEntity> ObtainEntity<TEntity>() where TEntity : class, IBehavioralEntity;
    bool TryGetKeyOf<TEntity, TKey>(TEntity entity, out TKey key) where TEntity : class, IBehavioralEntity;
}