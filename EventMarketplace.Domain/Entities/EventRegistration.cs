namespace EventMarketplace.Domain.Entities;

public class EventRegistration
{
    public Guid EventId { get; set; }
    public Event Event { get; set; }
    public Guid ParticipantId { get; set; }
    public User Participant { get; set; }
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}
