using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetTemplate.Core.Services
{
    public interface IStorage
    {
        Task<IReadOnlyCollection<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>> predicate,
            int? limit = null,
            CancellationToken token = default) where T : class;
        
        Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate,
            CancellationToken token = default) where T : class;
        
        Task AddEntityAsync<T>(T entity, CancellationToken token = default) where T : class;
        
        Task<bool> RemoveEntityAsync<T>(Expression<Func<T, bool>> predicate,
            CancellationToken token = default) where T : class;
        
        Task<bool> UpdateEntityAsync<T>(Expression<Func<T, bool>> predicate,
            Action<T> updateAction,
            CancellationToken token = default) where T : class;
    }
}