using Auto.Database.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auto.Database.UoW;

/// <summary>
/// Triển khai Unit of Work pattern để quản lý transaction
/// </summary>
public class UnitOfWork(AutoDbContext context) : IUnitOfWork
{
    private readonly AutoDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private IDbContextTransaction _transaction;
    private readonly Dictionary<Type, object> _repositories = [];
    private bool _disposed;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        catch (Exception)
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public IRepository<T> GetRepository<T>() where T : class
    {
        var type = typeof(T);

        if (!_repositories.TryGetValue(type, out object value))
        {
            value = new Repository<T>(_context);
            _repositories[type] = value;
        }

        return (IRepository<T>)value;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }

            _disposed = true;
        }
    }
}