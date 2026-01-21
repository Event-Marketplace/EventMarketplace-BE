using EventMarketplace.Application.Services.Events.CreateEvent;

namespace EventMarketplace.Application.UseCases.Events.CreateEvent;

public interface IEventManagementService
{
    Task CreateEvent(CreateEventRequest request, CancellationToken cancellationToken);
    Task DeleteEvent(Guid eventId);
}