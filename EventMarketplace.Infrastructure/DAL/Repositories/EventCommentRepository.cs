using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;

namespace EventMarketplace.Infrastructure.DAL.Repositories;

public class EventCommentRepository(EventMarketplaceDbContext context) : IEventCommentRepository
{
    public async Task AddEventCommentAsync(EventComment comment)
        => await context.EventComments.AddAsync(comment);
}