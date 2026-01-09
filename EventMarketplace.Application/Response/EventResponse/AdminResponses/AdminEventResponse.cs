namespace EventMarketplace.Application.Response.EventResponse.AdminResponses;

public class AdminEventResponse
{
    //event
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Duration { get; set; }
    public string Start { get; set; }
    public string End { get; set; }
    public string ImageUrl { get; set; }
    public string? Address { get; set; }
    public EventStatusResponse EventStatus { get; set; }

    public List<EventCommentResponse> Comments { get; set; }
    //organizer
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}