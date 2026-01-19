using EventMarketplace.Domain.Entities;

namespace EventMarketplace.Domain.Repositories;

public interface IEventCommentRepository
{
    Task AddEventCommentAsync(EventComment comment);
    Task UpdateEventCommentListAsync(List<EventComment> comments);
    Task<EventComment> GetEventCommentByIdAsync(Guid commentId);
    Task<List<EventComment>> GetEventCommentListByEventIdAsync(Guid eventId, Guid userId);
}