using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Auto.Database.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AutoGarageDbContext _context;
    protected readonly DbSet<T> _dbSet;

    /// <inheritdoc />
    public Repository(AutoGarageDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    /// <inheritdoc />
    public async Task<T> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    /// <inheritdoc />
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).ToListAsync();

    /// <inheritdoc />
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}