using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Infrastructure.DAL.DbOperations;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public sealed class GetEventsQueryHandler(EventMarketplaceDbContext context) : IQueryHandler<GetEventsQuery, EventListResponse>
{
    public async Task<EventListResponse> ExecuteHandleAsync(GetEventsQuery query, CancellationToken cancellationToken)
    {
        var events = context.Events
            .Where(x => x.IsActive)
            .FilterEvents(query)
            .OrderBy(x => x.DurationOfTheEvent.StartEvent);
            
        var paginatedResult = await events
            .PaginationEvents(query)
            .ToListAsync(cancellationToken: cancellationToken);

        return new EventListResponse()
        {
            Events = paginatedResult.Select(x => new EventResponse()
            {
                Id = x.Id,
                Description = x.Description,
                Price = x.Price,
                Title = x.Title,
                AvailableTickets = x.AvailableTickets,
                StartDate = x.DurationOfTheEvent.StartEvent,
                EndDate = x.DurationOfTheEvent.EndEvent,
                IsActive = x.IsActive,
                ImageUrl = x.ImageUrl,
                CreatedAt = x.CreateAt
            }).ToList(),
            TotalCount = events.Count()
        };
    }
}