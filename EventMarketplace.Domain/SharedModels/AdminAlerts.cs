namespace EventMarketplace.Domain.SharedModels;

public class AdminAlerts
{
    public List<AdminPendingEvents> PendingEvents { get; set; }
    public int PendingEventsCount { get; set; }
}