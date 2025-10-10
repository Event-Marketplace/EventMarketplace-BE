using EventMarketplace.Domain.Repositories;
using EventMarketplace.Infrastructure.DAL;
using EventMarketplace.Infrastructure.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventMarketplace.Application.Patterns;

public class UnitOfWork(EventMarketplaceDbContext context) : IUnitOfWork, IAsyncDisposable
{

    private IDbContextTransaction _transaction;


    public IUserRepository Users { get; } = new UserPostgresRepository(context);

    public async Task BeginTransactionAsync()
    {
        _transaction = await context.Database.BeginTransactionAsync();
    }

    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
    }

    public async Task CommitAsync()
    {
        await context.SaveChangesAsync();
        await _transaction.CommitAsync();
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