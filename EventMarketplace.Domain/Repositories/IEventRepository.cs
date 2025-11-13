using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Domain.Repositories;

public interface IEventRepository
{
    Task SetUnActiveByDate();
    Task AddEventAsync(Event eventEntity,CancellationToken cancellationToken);
    Task DeleteEventAsync(Event @event);
    Task<Event> GetEventByIdAsync(Guid id);
    Task DeleteEventByIdAsync(Guid id);
    Task UpdateEventAsync(Event @event);
}