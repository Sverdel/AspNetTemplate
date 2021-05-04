using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AspNetTemplate.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace AspNetTemplate.Infrastructure.DataAccess
{
    public class AspNetTemplateStorage : IStorage
    {
        private readonly IDbContextFactory<AspNetTemplateContext> _factory;

        public AspNetTemplateStorage(IDbContextFactory<AspNetTemplateContext> factory)
        {
            _factory = factory;
        }

        public async Task<IReadOnlyCollection<T>> GetEntitiesAsync<T>(Expression<Func<T, bool>> predicate,
            int? limit = null,
            CancellationToken token = default) where T : class
        {
            await using var context = _factory.CreateDbContext();
            var query = context.Set<T>()
                .AsNoTracking()
                .Where(predicate);

            if (limit != null)
            {
                query = query.Take(limit.Value);
            }

            return await query.ToListAsync(token);
        }

        public async Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken token) where T : class
        {
            await using var context = _factory.CreateDbContext();
            return await context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, token);
        }

        public async Task AddEntityAsync<T>(T entity, CancellationToken token) where T : class
        {
            await using var context = _factory.CreateDbContext();
            await context.Set<T>().AddAsync(entity, token);
            await context.SaveChangesAsync(token);
        }
        
        public async Task<bool> RemoveEntityAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken token) where T : class
        {
            await using var context = _factory.CreateDbContext();
            var entity = await context.Set<T>().FirstOrDefaultAsync(predicate, token);
            if (entity == null)
            {
                return false;
            }
            
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync(token);
            return true;
        }
        
        public async Task<bool> UpdateEntityAsync<T>(Expression<Func<T, bool>> predicate, Action<T> updateAction, CancellationToken token) where T : class
        {
            await using var context = _factory.CreateDbContext();
            var existing  = await context.Set<T>().FirstOrDefaultAsync(predicate, token);
            if (existing  == null)
            {
                return false;
            }

            updateAction(existing);
            await context.SaveChangesAsync(token);
            return true;
        }
    }
}