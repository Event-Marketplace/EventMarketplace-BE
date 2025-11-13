using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.Repositories;

public class EventPostgresRepository(EventMarketplaceDbContext context) : IEventRepository
{
    public async Task SetUnActiveByDate()
    {
        var events = await context.Events.ToListAsync();

        foreach (var item in events.Where(item => item.DurationOfTheEvent.StartEvent < DateTime.UtcNow && item.DurationOfTheEvent.EndEvent < DateTime.UtcNow))
        {
            item.IsActive = false;
        }

        context.Events.UpdateRange(events);
        await context.SaveChangesAsync();
    }

    public async Task AddEventAsync(Event eventEntity, CancellationToken cancellationToken)
    {
        await context.Events.AddAsync(eventEntity, cancellationToken);
    }

    public Task DeleteEventAsync(Event @event)
    {
        context.Events.Remove(@event);
        return Task.CompletedTask;
    }

    public async Task<Event> GetEventByIdAsync(Guid id)
        => await context.Events.SingleOrDefaultAsync(x => x.Id == id);

    public async Task DeleteEventByIdAsync(Guid id)
    {
        await context.Events.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public Task UpdateEventAsync(Event @event)
    {
        context.Events.Update(@event);
        return Task.CompletedTask;
    }
}