using Microsoft.EntityFrameworkCore.Storage;
using Notio.Common.Repositories.Async;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Nalix.Garage.Database.UoW;

/// <summary>
/// Triển khai Unit of Work pattern để quản lý transaction
/// </summary>
public class UnitOfWork(AutoDbContext context) : IUnitOfWorkAsync
{
    private readonly AutoDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private IDbContextTransaction _transaction;
    private readonly Dictionary<Type, object> _repositories = [];
    private bool _disposed;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

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

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

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

    public IRepositoryAsync<T> GetRepositoryAsync<T>() where T : class
    {
        if (!_repositories.TryGetValue(typeof(T), out var repository))
        {
            repository = new Repository<T>(_context);
            _repositories.Add(typeof(T), repository);
        }

        return (IRepositoryAsync<T>)repository;
    }
}