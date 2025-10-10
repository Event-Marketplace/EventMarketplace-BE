using EventMarketplace.Domain.Repositories;

namespace EventMarketplace.Application.Patterns;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    Task BeginTransactionAsync();
    Task RollbackAsync();
    Task CommitAsync();
}