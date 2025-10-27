using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SPSS.Shared.Base.Interfaces;
using System.Linq.Expressions;

namespace SPSS.Shared.Base.Implementations;

public class RepositoryBase<T, TKey> : IRepositoryBase<T, TKey> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public RepositoryBase(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    public IQueryable<T> Entities => _dbSet;

    public virtual async Task<T?> GetByIdAsync(TKey id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<T?> GetSingleAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        IQueryable<T> query = _dbSet;

        if (include != null)
        {
            query = include(query);
        }

        return await query.SingleOrDefaultAsync(predicate);
    }

    public virtual async Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (include != null)
        {
            query = include(query);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }

    public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be at least 1.");
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be at least 1.");

        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        int totalCount = await query.CountAsync();

        if (include != null)
        {
            query = include(query);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
    {
        if (filter != null)
        {
            return await _dbSet.CountAsync(filter);
        }
        return await _dbSet.CountAsync();
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>>? filter = null)
    {
        if (filter != null)
        {
            return await _dbSet.AnyAsync(filter);
        }
        return await _dbSet.AnyAsync();
    }

    public virtual void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public virtual void AddRange(IEnumerable<T> entities)
    {
        _dbSet.AddRange(entities);
    }

    public virtual void Update(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public virtual void Delete(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        T? entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            Delete(entity);
        }
    }

    public virtual void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public virtual void Attach(T entity)
    {
        _dbSet.Attach(entity);
    }

    public virtual void Detach(T entity)
    {
        _context.Entry(entity).State = EntityState.Detached;
    }
}