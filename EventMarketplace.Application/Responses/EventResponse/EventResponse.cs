using EventMarketplace.Domain.Enums;

namespace EventMarketplace.Application.Response.EventResponse;

public class EventResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public int AvailableTickets { get; set; }
    public string ImageUrl { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Guid OrganizerId { get; set; }
    public string Organizer { get; set; }
    public EventStatus Status { get; set; }
    public string StatusDisplayName { get; set; }
    public string LocationType { get; set; }
    public AddressResponse? AddressResponse { get; set; }
    public string? DescriptionEventPlace { get; set; }
    public List<EventCommentResponse> Comments { get; set; } = [];
    public bool WasRead { get; set; }
    public string? RejectionReason { get; set; }
}