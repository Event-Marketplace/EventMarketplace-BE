using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Domain.Repositories;

public interface IAuthRepository
{
    Task AddNewRefreshTokenAsync(RefreshToken refreshTokenEntity);
    Task<RefreshToken> GetEntityByRefreshTokenValue(string value);
    Task UpdateRefreshTokenEntity(RefreshToken refreshTokenEntity);
}