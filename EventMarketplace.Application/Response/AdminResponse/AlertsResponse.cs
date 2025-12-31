namespace EventMarketplace.Application.Response.AdminResponse;

public class AlertsResponse
{
    public List<PendingEventResponse> PendingEvents { get; set; }
    public int PendingEventsCount { get; set; }
}
