using Auto.Database.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auto.Database;

/// <summary>
/// Unit of Work pattern interface để quản lý transaction
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Lấy repository cho entity type
    /// </summary>
    IRepository<T> GetRepository<T>() where T : class;

    /// <summary>
    /// Bắt đầu một transaction mới
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commit transaction
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rollback transaction
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Lưu các thay đổi vào database
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}