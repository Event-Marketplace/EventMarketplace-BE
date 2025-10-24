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
        var events = await context.Events
            .FilterEvents(query)
            .OrderByDescending(x => x.StartDate)
            .ToListAsync(cancellationToken: cancellationToken);

        return new EventListResponse()
        {
            Events = events.Select(x => new EventResponse()
            {
                Id = x.Id,
                Description = x.Description,
                Price = x.Price,
                Title = x.Title,
                AvailableTickets = x.AvailableTickets,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                IsActive = x.IsActive,
                ImageUrl = x.ImageUrl,
                CreatedAt = x.CreateAt
            }).ToList(),
            TotalCount = events.Count
        };
    }
}