using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Application.Response.EventResponse.AdminResponses;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Infrastructure.DAL.DbOperations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers.AdminQueryHandlers;

public class GetAdminEventsQueryHandler(
    EventMarketplaceDbContext context) : IRequestHandler<GetAdminEventsQuery, AdminEventListResponse>
{
    public async Task<AdminEventListResponse> Handle(GetAdminEventsQuery request, CancellationToken cancellationToken)
    {
        var adminEvents = request.Tab switch
        {
            AdminEventListTab.Pending => context.Events.Where(x => x.EventStatus == EventStatus.Submitted),
            AdminEventListTab.Approved => context.Events.Where(x => x.EventStatus == EventStatus.Aproved),
            AdminEventListTab.Rejected => context.Events.Where(x => x.EventStatus == EventStatus.Rejected),
            AdminEventListTab.All => context.Events
        };

        var filteredQueryableEvents = adminEvents
            .FilterEvents(request)
            .AsNoTracking();
        
        var totalCount = filteredQueryableEvents.Count();
        
        var paginatedEvents = await filteredQueryableEvents
            .PaginationEvent(request)
            .Select(x => new AdminEventResponse()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                EventStatus = new EventStatusResponse()
                {
                    StatusIndex = (int)x.EventStatus,
                    StatusName = x.EventStatus.ToString(),
                    StatusDisplayName = x.EventStatus.GetDisplayName()
                },
                Address = x.LocationType == LocationType.Address ? x.Address.ToString() : x.EventPlaceDescription,
                Duration = x.DurationOfTheEvent.ToString(),
                ImageUrl = x.ImageUrl,
                FullName = x.Organizer.FullName.ToString(),
                Email = x.Organizer.EmailAddress.ToString(),
                Phone = x.Organizer.PhoneNumber.ToString()
                
            }).ToListAsync(cancellationToken: cancellationToken);
        
        return new AdminEventListResponse()
        {
            EventList = paginatedEvents,
            TotalCount = totalCount
        };
    }
}