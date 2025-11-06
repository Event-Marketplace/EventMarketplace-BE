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
}