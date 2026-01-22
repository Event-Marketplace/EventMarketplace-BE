using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Application.Services.Events.CreateEvent;

namespace EventMarketplace.Application.UseCases.Events.CreateEvent;

public interface IEventManagementService
{
    //Events
    Task CreateEvent(CreateEventRequest request, CancellationToken cancellationToken);
    Task DeleteEvent(Guid eventId, CancellationToken cancellationToken);
    Task EditEvent(EditEventRequest request, CancellationToken cancellationToken);
    
    //Event comments
    Task<EventCommentResponse> AddEventComment(AddEventCommentRequest request);
    Task ReadEventComments(Guid eventId);
}