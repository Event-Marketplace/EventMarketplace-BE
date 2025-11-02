using EventMarketplace.Application.Queries;
using EventMarketplace.Domain.Consts;
using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Infrastructure.DAL.DbOperations;

public static class EventOperations
{
    public static IQueryable<Event> FilterEvents(this IQueryable<Event> response, GetEventsQuery query)
    {
        //title
        if (!string.IsNullOrEmpty(query.Title))
        {
            response = response.Where(x => x.Title.ToLower().Contains(query.Title.ToLower()));
        }

        //date scope
        if (query.StartDate.HasValue)
            query.StartDate = DateTime.SpecifyKind(query.StartDate.Value, DateTimeKind.Utc);
        if (query.EndDate.HasValue)
            query.EndDate = DateTime.SpecifyKind(query.EndDate.Value, DateTimeKind.Utc);

        
        if (query.StartDate.HasValue && query.EndDate.HasValue)
        {
            response = response.Where(x => x.StartDate >= query.StartDate && x.EndDate <= query.EndDate);
        }else if (query.StartDate.HasValue && !query.EndDate.HasValue)
        {
            response = response.Where(x => x.StartDate >= query.StartDate);
        }else if (!query.StartDate.HasValue && query.EndDate.HasValue)
        {
            response = response.Where(x => x.EndDate <= query.EndDate);
        }

        //price scope
        if (query.StartPrice.HasValue && query.EndPrice.HasValue)
        {
            response = response.Where(x => x.Price >= query.StartPrice && x.Price <= query.EndPrice);
        }else if (query.StartPrice.HasValue && !query.EndPrice.HasValue)
        {
            response = response.Where(x => x.Price >= query.StartPrice);
        }else if (!query.StartPrice.HasValue && query.EndPrice.HasValue)
        {
            response = response.Where(x => x.Price <= query.EndPrice);
        }
        
        return response;
    }

    public static IQueryable<Event> PaginationEvents(this IQueryable<Event> response, GetEventsQuery query)
    {
        response = query.PageNumber > 0 ? response.Skip(((query.PageNumber - 1) * Variables.PAGE_SIZE)).Take(Variables.PAGE_SIZE): response.Take(Variables.PAGE_SIZE);
        return response;
    }
}