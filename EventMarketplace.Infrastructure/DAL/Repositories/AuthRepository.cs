using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.Repositories;

public class AuthRepository(EventMarketplaceDbContext context) : IAuthRepository
{
    public async Task AddNewRefreshTokenAsync(RefreshToken refreshTokenEntity)
    {
        await context.RefreshTokens.AddAsync(refreshTokenEntity);
    }

    public async Task<RefreshToken> GetEntityByRefreshTokenValue(string value)
        => await context.RefreshTokens.SingleOrDefaultAsync(x => x.Value.Equals(value));
    
    public async Task UpdateRefreshTokenEntity(RefreshToken refreshTokenEntity)
    {
        context.RefreshTokens.Update(refreshTokenEntity);
    }
}