using EventMarketplace.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.Repositories;

public class EventPostgresRepository(EventMarketplaceDbContext context) : IEventRepository
{
    public async Task SetUnActiveByDate()
    {
        var events = await context.Events.ToListAsync();

        foreach (var item in events.Where(item => item.StartDate < DateTime.UtcNow && item.EndDate < DateTime.UtcNow))
        {
            item.IsActive = false;
        }

        context.Events.UpdateRange(events);
        await context.SaveChangesAsync();
    }
}