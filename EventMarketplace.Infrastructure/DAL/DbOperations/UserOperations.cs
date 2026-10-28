using EventMarketplace.Application.Queries;
using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Infrastructure.DAL.DbOperations;

public static class UserOperations
{
    public static IQueryable<User> FilterUsers(this IQueryable<User> response, GetUsersQuery query)
    {
        if (!string.IsNullOrEmpty(query.FullName))
        {
            response = response.Where(x =>
                x.FullName.FirstName.Contains(query.FullName, StringComparison.CurrentCultureIgnoreCase)
                || x.FullName.LastName.Contains(query.FullName, StringComparison.CurrentCultureIgnoreCase));
        }

        // if (query.Roles != null && query.Roles.Any())
        // {
        //     var roleIds = query.Roles.ToHashSet();
        //     response = response.Where(x => x.UserRoles.Any(ur => roleIds.Contains(ur.RoleId)));
        // }
        
        if (query.Roles != null && query.Roles.Any())
        {
            response = response.Where(x => x.UserRoles.Any(ur => query.Roles.Contains(ur.RoleId)));
        }
        
        //if(query.UserActivity)
        if (!string.IsNullOrEmpty(query.Address))
        {
            
        }
        
        return response;
    }
}