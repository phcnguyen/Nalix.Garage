using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Auto.Database.Repositories;

/// <summary>
/// Generic repository interface for asynchronous CRUD operations and querying entities.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public interface IRepositoryAsync<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "",
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    void Update(T entity);

    void UpdateRange(IEnumerable<T> entities);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    void Delete(T entity);

    void DeleteRange(IEnumerable<T> entities);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}