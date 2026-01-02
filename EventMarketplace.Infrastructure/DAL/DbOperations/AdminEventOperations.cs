using EventMarketplace.Application.Queries;
using EventMarketplace.Domain.Consts;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;

namespace EventMarketplace.Infrastructure.DAL.DbOperations;

public static class AdminEventOperations
{
    public static IQueryable<Event> FilterEvents(this IQueryable<Event> response, GetAdminEventsQuery query)
    {
        if (query.EventStatuses != null && query.Tab == AdminEventListTab.All)
        {
            response = response.Where(x => query.EventStatuses.Contains(x.EventStatus));
        }

        if (!string.IsNullOrEmpty(query.TitleFilter))
        {
            response = response.Where(x => x.Title.ToLower().Contains(query.TitleFilter.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.OrganizerFilter))
        {
            response = response.Where(x =>
                x.Organizer.FullName.FirstName.ToLower().Contains(query.OrganizerFilter.ToLower()) ||
                x.Organizer.FullName.LastName.ToLower().Contains(query.OrganizerFilter.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.CityFilter))
        {
            response = response.Where(x => x.Address.City.ToLower().Contains(query.CityFilter.ToLower()));
        }
        
        if (query.CreatedFromFilter.HasValue && query.CreatedToFilter.HasValue)
        {
            response = response.Where(x => x.CreatedAt.Date >= query.CreatedFromFilter && x.CreatedAt.Date <= query.CreatedToFilter);
        }else if (query.CreatedFromFilter.HasValue && !query.CreatedToFilter.HasValue)
        {
            response = response.Where(x => x.CreatedAt.Date >= query.CreatedFromFilter);
        }else if (!query.CreatedFromFilter.HasValue && query.CreatedToFilter.HasValue)
        {
            response = response.Where(x => x.CreatedAt.Date <= query.CreatedToFilter);
        }
        
        return response;
    }

    public static IQueryable<Event> PaginationEvent(this IQueryable<Event> response, GetAdminEventsQuery query)
    {
        response = query.PageNumber > 0 ? response.Skip(((query.PageNumber - 1) * Variables.PAGE_SIZE)).Take(Variables.PAGE_SIZE): response.Take(Variables.PAGE_SIZE);
        return response;
    }
}