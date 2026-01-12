using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Domain.Repositories;

public interface IEventRepository
{
    Task AddEventAsync(Event eventEntity,CancellationToken cancellationToken);
    Task DeleteEventAsync(Event @event);
    Task<Event> GetEventByIdAsync(Guid id);
    Task DeleteEventByIdAsync(Guid id);
    Task UpdateEventAsync(Event @event);
    Task<bool> CheckIsEventExist(Guid eventId);
    Task<bool> ExistsByIdAndOwner(Guid eventId, Guid ownerId);
}