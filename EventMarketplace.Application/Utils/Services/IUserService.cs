using EventMarketplace.Domain.Enums;

namespace EventMarketplace.Application.Utils;

public interface IUserService
{
    Guid GetUserIdFromContext();
    Task<string> GetCurrentUserFullName(Guid userId);
    string GetUserEmailFromContext();
    bool IsInRole(RoleType roleType);
    Task<bool> CanUserAccessEvent(Guid eventId, Guid userId);
}