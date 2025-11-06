using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Domain.Repositories;

public interface IEventRepository
{
    Task SetUnActiveByDate();
    Task AddEventAsync(Event eventEntity,CancellationToken cancellationToken);
}