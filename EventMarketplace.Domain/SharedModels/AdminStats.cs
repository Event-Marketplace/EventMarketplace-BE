namespace EventMarketplace.Domain.SharedModels;

public class AdminStats
{
    public int TotalEvents { get; set; }
    public int ApprovedEvents { get; set; }
    public int RejectedEvents { get; set; }
    public int PendingEvents { get; set; }
    public int TotalUsers { get; set; }
    public int TotalOrganizers { get; set; }
    public int TotalParticipants { get; set; }
}