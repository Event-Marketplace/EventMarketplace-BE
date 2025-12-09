using EventMarketplace.Application.Queries;
using EventMarketplace.Domain.Consts;
using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Infrastructure.DAL.DbOperations;

public static class OrganizerEventOperations
{
    public static IQueryable<Event> FilterEvents(this IQueryable<Event> response, GetOrganizerEventsQuery query)
    {
        //title
        if (!string.IsNullOrEmpty(query.Title))
        {
            response = response.Where(x => x.Title.ToLower().Contains(query.Title.ToLower()));
        }

        //status
        if (query.Status != null)
        {
            response = response.Where(x => x.EventStatus == query.Status);
        }

        //dates
        if (query.StartDate.HasValue)
            query.StartDate = DateTime.SpecifyKind(query.StartDate.Value, DateTimeKind.Utc);
        if (query.EndDate.HasValue)
            query.EndDate = DateTime.SpecifyKind(query.EndDate.Value, DateTimeKind.Utc);

        if (query.StartDate.HasValue && query.EndDate.HasValue)
        {
            response = response.Where(x =>
                x.DurationOfTheEvent.StartEvent >= query.StartDate && x.DurationOfTheEvent.EndEvent <= query.EndDate);
        }else if (!query.StartDate.HasValue && query.EndDate.HasValue)
        {
            response = response.Where(x => x.DurationOfTheEvent.EndEvent <= query.EndDate);
        }else if (query.StartDate.HasValue && !query.EndDate.HasValue)
        {
            response = response.Where(x => x.DurationOfTheEvent.StartEvent >= query.StartDate);
        }
        
        return response;
    }
    
    public static IQueryable<Event> PaginationEvents(this IQueryable<Event> response, GetOrganizerEventsQuery query)
    {
        response = query.PageNumber > 0 ? response.Skip(((query.PageNumber - 1) * Variables.PAGE_SIZE)).Take(Variables.PAGE_SIZE): response.Take(Variables.PAGE_SIZE);
        return response;
    }
}