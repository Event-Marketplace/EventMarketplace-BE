using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Response;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EventMarketplace.Application.Utils.Jwt;

public class JwtProvider(IHttpContextAccessor contextAccessor, IUserRepository userRepository) : IJwtProvider
{
    public string GenerateToken(User user)
    {
        var userRolesNames = userRepository.GetUserRoles(user.Id);
        
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress.Value),
        };
        
        claims.AddRange(userRolesNames.Select(role => new Claim(ClaimTypes.Role, role)));

        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        var jwtExp = Environment.GetEnvironmentVariable("JWT_EXPIRES");

        if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience) || string.IsNullOrEmpty(jwtExp))
            throw new EmException("Brak zmiennych środowiskowych dla tokena JWT.","NO_ENVIRONMENTS");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtExp)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshTokenResponse GenerateRefreshToken(User user)
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        return new RefreshTokenResponse()
        {
            RefreshToken = Convert.ToBase64String(bytes),
            Expires = DateTime.UtcNow.AddDays(7)
        };
    }

    public void AppendRefreshToken(string refreshToken)
    {
        CheckUserSessionExist();
        var cookiesOptions = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true
        };
        
        contextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookiesOptions);
    }

    public string GetRefreshTokenFromCookies()
    {
        CheckUserSessionExist();
        var refreshToken = contextAccessor.HttpContext.Request.Cookies["refreshToken"] 
                           ?? throw new EmException("Brak refresh token'a","NO_REFRESH_TOKEN");

        return refreshToken;
    }

    public void SetNullRefreshTokenInCookies()
    {
        contextAccessor.HttpContext.Response.Cookies.Append("refreshToken", "", new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(-1)
        });
    }

    private void CheckUserSessionExist()
    {
        if (contextAccessor.HttpContext == null) throw new EmException("Brak sesji użytkownika.","NO_SESSION");
    }
}