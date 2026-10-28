using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;

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
    Task<EventRegistrationResult> RegisterParticipantAsync(Guid eventId, Guid participantId, CancellationToken cancellationToken);
}
