using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Domain.Repositories;

public interface IAuthRepository
{
    Task AddNewRefreshTokenAsync(RefreshToken refreshTokenEntity);
    Task UpdateRefreshTokenEntity(RefreshToken refreshTokenEntity);
}