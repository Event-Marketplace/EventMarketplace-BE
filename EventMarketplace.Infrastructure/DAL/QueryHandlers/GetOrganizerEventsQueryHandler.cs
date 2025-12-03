using System.Security.Claims;
using AutoMapper;
using EventMarketplace.Application.Commands.EventCommands.CreateEvent;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public class GetOrganizerEventsQueryHandler(EventMarketplaceDbContext context, IHttpContextAccessor contextAccessor, IMapper mapper) : IRequestHandler<GetOrganizerEventsQuery, EventListResponse>
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
            Events = organizerEvents
                .Select(x => mapper.Map<EventResponse>(x))
                .ToList(),
            TotalCount = organizerEvents.Count
        };
    }
}