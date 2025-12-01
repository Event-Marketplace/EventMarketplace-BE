using EventMarketplace.Domain.Repositories;

namespace EventMarketplace.Application.Patterns;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IEventRepository Events { get; }
    IAuthRepository AuthRepo { get; }
    IRoleRepository RoleRepo { get; }
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
}