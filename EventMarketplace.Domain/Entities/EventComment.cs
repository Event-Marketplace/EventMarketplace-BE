namespace EventMarketplace.Domain.Entities;

public class EventComment : BaseEntity
{
    public string Content { get; set; }
    public bool WasReadByAdmin { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid EventId { get; set; }
    public Event Event { get; set; }
    
}