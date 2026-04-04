using EventMarketplace.Application.Services.Events.CreateEvent;
using EventMarketplace.Application.UseCases.Events.CreateEvent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EventMarketplace.Application.Utils.SignalR;

[Authorize(Roles = "Admin,Organizer")]
public class EventHub(
    IEventManagementService eventManagementService) : Hub
{
    public async Task AddComment(Guid eventId, string comment, string currentContext)
    {
        var request = new AddEventCommentRequest()
            { EventId = eventId, Comment = comment, CurrentContext = currentContext };
        var lastComment = await eventManagementService.AddEventComment(request);
        
        await Clients.Group($"Event_{eventId}").SendAsync("ReceiveComment", eventId, lastComment);
    }

    
    public async Task JoinEventGroup(Guid eventId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Event_{eventId}");
    }
    
    public async Task LeaveEventGroup(Guid eventId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Event_{eventId}");
    }

    public async Task ReadComments(Guid eventId)
    {
        await eventManagementService.ReadEventComments(eventId);
        await Clients.Group($"Event_{eventId}").SendAsync("CommentsRead", eventId);
    }
}
