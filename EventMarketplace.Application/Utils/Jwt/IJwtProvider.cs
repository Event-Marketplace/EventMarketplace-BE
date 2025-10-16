using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Application.Utils.Jwt;

public interface IJwtProvider
{
    string GenerateToken(User user);
}