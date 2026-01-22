using EventMarketplace.Domain.Enums;

namespace EventMarketplace.Application.Services.Events.CreateEvent;

public class AddEventCommentRequest
{
    public Guid EventId { get; set; }
    public string Comment { get; set; }
    public string CurrentContext { get; set; }
}