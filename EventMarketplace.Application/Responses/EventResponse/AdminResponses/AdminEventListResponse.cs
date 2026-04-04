namespace EventMarketplace.Application.Response.EventResponse.AdminResponses;

public class AdminEventListResponse
{
    public List<AdminEventResponse> EventList { get; set; }
    public int TotalCount { get; set; }
}