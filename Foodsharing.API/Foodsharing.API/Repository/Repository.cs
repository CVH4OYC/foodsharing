using Foodsharing.API.Data;
using Foodsharing.API.Interfaces;
using Foodsharing.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Foodsharing.API.Repository;

/// <summary>
/// Класс универсального репозитория, реализующий IRepository
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext context;
    public Repository(AppDbContext context)
    {
        this.context = context;
    }
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Set<T>().ToListAsync(cancellationToken); ;
    }

    public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.FindAsync<T>(id, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        // прикрепляем к контексту без отслеживания изменений
        context.Set<T>().Attach(entity);
        // говорим, что сущность изменена
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Deleted;
        await context.SaveChangesAsync(cancellationToken);
    }
    public async Task<IEnumerable<T>> GetWithIncludeAsync(CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includeProperties)
    {
        return await Include(includeProperties).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetWithIncludeAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var query = Include(includeProperties);
        return await query.Where(predicate).ToListAsync(cancellationToken);
    }

    private IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = context.Set<T>().AsNoTracking();
        return includeProperties
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }

    private bool disposed = false;
    public virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
