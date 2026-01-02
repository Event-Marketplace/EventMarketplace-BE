using System.Security.Claims;
using AutoMapper;
using EventMarketplace.Application.Commands.EventCommands.CreateEvent;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Infrastructure.DAL.DbOperations;
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

        var organizerEvents = context.Events
            .Include(x => x.Organizer)
            .Where(x => x.OrganizerId == organizer.Id)
            .FilterEvents(request)
            .OrderByDescending(x => x.CreatedAt);

        var paginatedResult = await organizerEvents
            .PaginationEvents(request)
            .Select(x => mapper.Map<EventResponse>(x))
            .ToListAsync(cancellationToken);

        return new EventListResponse()
        {
            Events = paginatedResult,
            TotalCount = organizerEvents.Count()
        };
    }
}