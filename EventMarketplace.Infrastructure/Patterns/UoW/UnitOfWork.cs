using EventMarketplace.Domain.Repositories;
using EventMarketplace.Infrastructure.DAL;
using EventMarketplace.Infrastructure.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventMarketplace.Application.Patterns;

public class UnitOfWork(EventMarketplaceDbContext context) : IUnitOfWork, IAsyncDisposable
{

    private IDbContextTransaction _transaction;

    public IUserRepository Users { get; } = new UserPostgresRepository(context);
    public IEventRepository Events { get; } = new EventPostgresRepository(context);
    public IAuthRepository Auths { get; } = new AuthRepository(context);
    public IRoleRepository Roles { get; } = new RoleRepository(context);
    public IEventCommentRepository EventComments { get; } = new EventCommentRepository(context);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await _transaction.RollbackAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
        await _transaction.CommitAsync(cancellationToken);
    }

    public void Dispose()
    {
        _transaction.Dispose();
        context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        if (_transaction != null)
        {
            await context.DisposeAsync();
        }
    }
}