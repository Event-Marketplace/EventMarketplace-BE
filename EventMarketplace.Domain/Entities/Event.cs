using EventMarketplace.Domain.Enums;
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
    public DateTime? UpdatedAt { get; set; }
    public EventStatus EventStatus { get; set; }
    public Guid OrganizerId { get; set; }
    public User Organizer { get; set; }
    public LocationType LocationType { get; set; }
    public Address? Address { get; set; }
    public string? EventPlaceDescription { get; set; }
    public string? RejectionReason { get; set; }
    public ICollection<EventComment> EventComments { get; set; } = new List<EventComment>();
    
    public static Event Create(string title, string description, double price, int availableTickets, 
        DateTime start, DateTime end, Guid organizerId, string imageUrl, Address? address, string? eventPlaceDescription, LocationType locationType)
    {
        return new Event()
        {
            Id = Guid.CreateVersion7(),
            Title = title,
            Description = description,
            Price = price,
            AvailableTickets = availableTickets,
            DurationOfTheEvent = DurationOfTheEvent.Create(start, end),
            OrganizerId = organizerId,
            ImageUrl = imageUrl,
            CreatedAt = DateTime.UtcNow,
            Address = address,
            EventPlaceDescription = eventPlaceDescription,
            EventStatus = EventStatus.Draft,
            LocationType = locationType
        };
    }

    public void SubmitEventToAdminVerification()
    {
        EventStatus = EventStatus.Submitted;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ApproveEvent()
    {
        EventStatus = EventStatus.Aproved;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RejectEvent(string reason)
    {
        EventStatus = EventStatus.Rejected;
        RejectionReason = reason;
        UpdatedAt = DateTime.UtcNow;
    }
}