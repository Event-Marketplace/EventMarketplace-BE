namespace EventMarketplace.Application.Response.EventResponse;

public class EventListResponse
{
    public List<EventResponse> Events { get; set; }
    public int TotalCount { get; set; }
}