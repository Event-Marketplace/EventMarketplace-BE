using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Domain.Repositories;

public interface IEventCommentRepository
{
    Task AddEventCommentAsync(EventComment comment);
}