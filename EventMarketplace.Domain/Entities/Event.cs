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
    
    public static Event Create(string title, string description, double price, int availableTickets, 
        DateTime start, DateTime end, Guid organizerId, string imageUrl)
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
            CreateAt = DateTime.UtcNow,
        };
    }

    public void SetIsActiveEvent()
    {
        IsActive = DurationOfTheEvent.StartEvent.Date >= DateTime.UtcNow.Date;
    }
}