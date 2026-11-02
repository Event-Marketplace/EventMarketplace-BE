using EventMarketplace.Application.Queries;
using EventMarketplace.Domain.Consts;
using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Infrastructure.DAL.DbOperations;

public static class UserOperations
{
    public static IQueryable<User> FilterUsers(this IQueryable<User> response, GetUsersQuery query)
    {
        if (!string.IsNullOrEmpty(query.FullName))
        {
            response = response.Where(x =>
                x.FullName.FirstName.ToLower().Contains(query.FullName.ToLower())
                || x.FullName.LastName.ToLower().Contains(query.FullName.ToLower()));
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

    public static IQueryable<User> PaginateUsers(this IQueryable<User> response, GetUsersQuery query)
    {
        var pageNumber = Math.Max(query.PageNumber, 1);
        return response
            .OrderByDescending(user => user.CreatedAt)
            .Skip((pageNumber - 1) * Variables.PAGE_SIZE)
            .Take(Variables.PAGE_SIZE);
    }
}
