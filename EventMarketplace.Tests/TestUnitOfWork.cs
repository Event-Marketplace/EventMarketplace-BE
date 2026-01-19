using EventMarketplace.Application.Patterns;
using EventMarketplace.Domain.Repositories;
using EventMarketplace.Infrastructure.DAL;
using EventMarketplace.Infrastructure.DAL.Repositories;

namespace EventMarketplace.Tests;

public class TestUnitOfWork(EventMarketplaceDbContext context) : IUnitOfWork, IAsyncDisposable
{
    public IUserRepository Users => new UserPostgresRepository(context);
    public IEventRepository Events => new EventPostgresRepository(context);
    public IAuthRepository Auths => new AuthRepository(context);
    public IRoleRepository Roles => new RoleRepository(context);
    public IEventCommentRepository EventComments { get; } = new EventCommentRepository(context);

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task RollbackAsync(CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task CommitAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
    
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
    

    public void Dispose()
    {
        context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }
}