using System.Security.Claims;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace EventMarketplace.Application.Utils;

public class UserService(IHttpContextAccessor accessor, IEventRepository eventRepository, IUserRepository userRepository) : IUserService
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

    public async Task<string> GetCurrentUserFullName(Guid userId)
        => await userRepository.GetUserFullNameByIdAsync(userId);

    public string GetUserEmailFromContext()
    {
        var value = accessor?.HttpContext?.User.FindFirst(ClaimTypes.Email).Value;
        if (value == null)
        {
            throw new AppException("User is not Authenticated!");
        }

        return value;
    }

    public bool IsInRole(RoleType roleType)
    {
        var claimsPrincipal = accessor?.HttpContext?.User;
        
        if (claimsPrincipal == null)
            throw new AppException("User is not Authenticated!");

        var roles = claimsPrincipal?
            .Claims
            .Where(c => c.Type == ClaimTypes.Role) 
            .Select(c => c.Value)
            .ToList();

        return roles.Contains(roleType.ToString());
    }

    public async Task<bool> CanUserAccessEvent(Guid eventId, Guid userId)
    {
        return await eventRepository.ExistsByIdAndOwner(eventId, userId);
    }
}