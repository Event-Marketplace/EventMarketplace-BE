namespace EventMarketplace.Application.Response.EventResponse;

public class EventCommentResponse
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; }
    public string User { get; set; }
    public string CreatedAt { get; set; }
}