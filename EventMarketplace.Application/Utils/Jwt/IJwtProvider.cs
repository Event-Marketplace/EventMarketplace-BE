using EventMarketplace.Application.Response;
using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Application.Utils.Jwt;

public interface IJwtProvider
{
    string GenerateToken(User user);
    RefreshTokenResponse GenerateRefreshToken(User user);
    void AppendRefreshToken(string refreshToken);
    string GetRefreshTokenFromCookies();
    void SetNullRefreshTokenInCookies();
}