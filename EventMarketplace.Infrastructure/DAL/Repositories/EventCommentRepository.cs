using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.Repositories;

public class EventCommentRepository(EventMarketplaceDbContext context) : IEventCommentRepository
{
    public async Task AddEventCommentAsync(EventComment comment)
        => await context.EventComments.AddAsync(comment);

    public async Task<EventComment> GetLastCommentByEventAndUser(Guid eventId, Guid userId)
        => await context.EventComments
            .Include(x => x.User)
            .Where(x => x.EventId == eventId && x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    
}