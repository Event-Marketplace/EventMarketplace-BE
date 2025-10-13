using EventMarketplace.Domain.Repositories;
using EventMarketplace.Infrastructure.DAL;
using EventMarketplace.Infrastructure.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventMarketplace.Application.Patterns;

public class UnitOfWork(EventMarketplaceDbContext context) : IUnitOfWork, IAsyncDisposable
{

    private IDbContextTransaction _transaction;


    public IUserRepository Users { get; } = new UserPostgresRepository(context);

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
        await _transaction.DisposeAsync();
        await context.DisposeAsync();
    }
}