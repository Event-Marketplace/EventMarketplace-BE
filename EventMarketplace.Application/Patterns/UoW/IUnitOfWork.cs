using EventMarketplace.Domain.Repositories;

namespace EventMarketplace.Application.Patterns;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IEventRepository Events { get; }
    IAuthRepository Auths { get; }
    IRoleRepository Roles { get; }
    IEventCommentRepository EventComments { get; }
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}