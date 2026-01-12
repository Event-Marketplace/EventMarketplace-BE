using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.Repositories;

public class EventCommentRepository(EventMarketplaceDbContext context) : IEventCommentRepository
{
    public async Task AddEventCommentAsync(EventComment comment)
        => await context.EventComments.AddAsync(comment);
    
    public async Task<EventComment> GetEventCommentByIdAsync(Guid commentId)
        => await context.EventComments
            .Include(x => x.User)
            .Where(x => x.Id == commentId)
            .SingleOrDefaultAsync();
    
}