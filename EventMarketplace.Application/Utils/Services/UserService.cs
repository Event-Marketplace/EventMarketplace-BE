using System.Security.Claims;
using EventMarketplace.Application.Exceptions;
using Microsoft.AspNetCore.Http;

namespace EventMarketplace.Application.Utils;

public class UserService(IHttpContextAccessor accessor) : IUserService
{
    public Guid GetUserIdFromContext()
    {
        var value = accessor?
            .HttpContext?
            .User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (value == null)
            throw new AppException("User is not Authenticated!");
        
        return Guid.Parse(value);
    }
}