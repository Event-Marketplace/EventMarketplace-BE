using EventMarketplace.Domain.ValueObjects;

namespace EventMarketplace.Domain.Entities;

public class Event : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DurationOfTheEvent DurationOfTheEvent {get; set; }
    public double Price { get; set; }
    public int AvailableTickets { get; set; }
    public string ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public Guid OrganizerId { get; set; }
    public User Organizer { get; set; }
}