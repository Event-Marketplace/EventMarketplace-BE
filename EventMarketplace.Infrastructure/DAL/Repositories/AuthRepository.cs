using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;

namespace EventMarketplace.Infrastructure.DAL.Repositories;

public class AuthRepository(EventMarketplaceDbContext context) : IAuthRepository
{
    public async Task AddNewRefreshTokenAsync(RefreshToken refreshTokenEntity)
    {
        await context.RefreshTokens.AddAsync(refreshTokenEntity);
    }

    public async Task UpdateRefreshTokenEntity(RefreshToken refreshTokenEntity)
    {
        context.RefreshTokens.Update(refreshTokenEntity);
    }
}