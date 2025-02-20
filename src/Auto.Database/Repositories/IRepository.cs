using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Auto.Database.Repositories;

/// <summary>
/// Generic repository interface for CRUD operations.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains an enumerable of entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Retrieves an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the entity.</returns>
    Task<T> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves entities that match a specified condition.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains an enumerable of matching entities.</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(T entity);

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(int id);

    /// <summary>
    /// Saves all changes made to the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync();
}