
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Infrastructure.DAL.DbOperations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public sealed class GetEventsQueryHandler(EventMarketplaceDbContext context) : IRequestHandler<GetEventsQuery, EventListResponse>
{
    public async Task<EventListResponse> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var events = context.Events
            .Include(x => x.Organizer)
            .Where(x => x.EventStatus == EventStatus.Approved)
            .FilterEvents(request)
            .OrderBy(x => x.DurationOfTheEvent.StartEvent);
            
        var paginatedResult = await events
            .PaginationEvents(request)
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
                Status = x.EventStatus,
                StatusDisplayName = x.EventStatus.GetDisplayName(),
                ImageUrl = x.ImageUrl,
                CreatedAt = x.CreatedAt,
                OrganizerId = x.OrganizerId,
                Organizer = x.Organizer.FullName.ToString()
            }).ToList(),
            TotalCount = events.Count()
        };
    }
}