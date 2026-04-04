namespace EventMarketplace.Application.Response.AdminResponse;

public class StatsResponse
{
    public int TotalUsers { get; set; }
    public int ApprovedEvents { get; set; }
    public int PendingEvents { get; set; }
    public int RejectedEvents { get; set; }
    public int TotalOrganizers { get; set; }
    public int TotalParticipants { get; set; }
    public int TotalEvents { get; set; }
}