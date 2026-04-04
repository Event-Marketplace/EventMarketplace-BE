using AutoMapper;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Infrastructure.DAL.DbOperations;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public class EventReadModel(EventMarketplaceDbContext context, IUserService userService, IMapper mapper) : IEventReadModel
{
    public async Task<EventListResponse> GetEventsList(GetEventsQuery query, CancellationToken cancellationToken)
    {
        var events = context.Events
            .Include(x => x.Organizer)
            .Where(x => x.EventStatus == EventStatus.Approved)
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

    public async Task<EventListResponse> GetOrganizerEventsList(GetOrganizerEventsQuery query, CancellationToken cancellationToken)
    { 
        var loggedOrganizerId = userService.GetUserIdFromContext();
        var organizer = await context.Users
                            .SingleOrDefaultAsync(x => x.Id == loggedOrganizerId, cancellationToken: cancellationToken) 
                        ?? throw new EmNotFoundException($"No organizer with this ID - {loggedOrganizerId}");

        var organizerEvents = context.Events
            .Include(x => x.Organizer)
            .Include(x => x.EventComments)
            .ThenInclude(x => x.User)
            .Where(x => !x.IsDeleted && x.OrganizerId == organizer.Id)
            .FilterEvents(query)
            .OrderByDescending(x => x.CreatedAt);

        var paginatedResult = await organizerEvents
            .PaginationEvents(query)
            .Select(x => mapper.Map<EventResponse>(x))
            .ToListAsync(cancellationToken);

        return new EventListResponse()
        {
            Events = paginatedResult,
            TotalCount = organizerEvents.Count()
        };
    }
}