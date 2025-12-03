using System.Security.Claims;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public class GetOrganizerEventsQueryHandler(EventMarketplaceDbContext context, IHttpContextAccessor contextAccessor) : IRequestHandler<GetOrganizerEventsQuery, EventListResponse>
{
    public async Task<EventListResponse> Handle(GetOrganizerEventsQuery request, CancellationToken cancellationToken)
    {
        var loggedOrganizerId = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var organizer = await context.Users
            .SingleOrDefaultAsync(x => x.Id == Guid.Parse(loggedOrganizerId), cancellationToken: cancellationToken) 
            ?? throw new AppException("Brak organizatora o takim id.");

        var organizerEvents = await context.Events
            .Include(x => x.Organizer)
            .Where(x => x.OrganizerId == organizer.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        return new EventListResponse()
        {
            Events = organizerEvents.Select(x => new EventResponse()
            {
                Id = x.Id,
                Description = x.Description,
                Price = x.Price,
                Title = x.Title,
                AvailableTickets = x.AvailableTickets,
                StartDate = x.DurationOfTheEvent.StartEvent,
                EndDate = x.DurationOfTheEvent.EndEvent,
                Status = x.EventStatus.GetDisplayName(),
                ImageUrl = x.ImageUrl,
                CreatedAt = x.CreateAt,
                OrganizerId = x.OrganizerId,
                Organizer = x.Organizer.FullName.ToString()
            }).ToList(),
            TotalCount = organizerEvents.Count
        };
    }
}