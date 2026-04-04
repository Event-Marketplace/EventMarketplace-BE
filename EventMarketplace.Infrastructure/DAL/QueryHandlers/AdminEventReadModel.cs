using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Queries.AdminQueries;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Application.Response.EventResponse.AdminResponses;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Infrastructure.DAL.DbOperations;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public class AdminEventReadModel(EventMarketplaceDbContext context) : IAdminEventReadModel
{
    public async Task<AdminEventListResponse> GetAdminEventListAsync(GetAdminEventsQuery query, CancellationToken cancellationToken)
    {
         var adminEvents = query.Tab switch
        {
            AdminEventListTab.Pending => context.Events.Where(x => x.EventStatus == EventStatus.Submitted),
            AdminEventListTab.Approved => context.Events.Where(x => x.EventStatus == EventStatus.Approved),
            AdminEventListTab.Rejected => context.Events.Where(x => x.EventStatus == EventStatus.Rejected),
            AdminEventListTab.All => context.Events
        };

        var filteredQueryableEvents = adminEvents
            .FilterEvents(query)
            .AsNoTracking();
        
        var totalCount = await filteredQueryableEvents.CountAsync(cancellationToken: cancellationToken);
        
        var paginatedEvents = await filteredQueryableEvents
            .PaginationEvent(query)
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
                Duration = DurationUtils.DurationCalculate(x.DurationOfTheEvent.StartEvent, x.DurationOfTheEvent.EndEvent),
                Start = x.DurationOfTheEvent.StartEvent.ToString("dd.MM.yyyy, hh:mm"),
                End = x.DurationOfTheEvent.EndEvent.ToString("dd.MM.yyyy, hh:mm"),
                ImageUrl = x.ImageUrl,
                FullName = x.Organizer.FullName.ToString(),
                Email = x.Organizer.EmailAddress.ToString(),
                Phone = x.Organizer.PhoneNumber.ToString(),
                Comments = x.EventComments.Select(ec => new EventCommentResponse()
                {
                    Id = ec.Id,
                    Content = ec.Content,
                    User = ec.User.FullName.ToString(),
                    CreatedAt = ec.CreatedAt.ToString("dd.MM.yyyy hh:ss"),
                    EventId = ec.EventId,
                    UserId = ec.UserId,
                    WasRead = ec.WasReadByAdmin
                }).ToList(),
                RejectionReason = x.RejectionReason
                
            }).ToListAsync(cancellationToken: cancellationToken);
        
        return new AdminEventListResponse()
        {
            EventList = paginatedEvents,
            TotalCount = totalCount
        };
    }
}