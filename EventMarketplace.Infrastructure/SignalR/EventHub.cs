using EventMarketplace.Application.Commands.EventCommands;
using EventMarketplace.Application.Commands.EventCommands.AdminFunctions;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventMarketplace.Application.Utils.SignalR;

public class EventHub(IMediator mediator, IUserService userService) : Hub
{
    public async Task RejectEvent(Guid eventId, string comment)
    {
        await mediator.Send(new RejectEventCommand(eventId, comment));
        await Clients.Group($"Event_{eventId}").SendAsync("RejectedEvent", eventId, comment, userService.GetUserIdFromContext());
    }
    
    public async Task AddComment(Guid eventId, string comment)
    {
        await mediator.Send(new AddEventCommentCommand(eventId, comment));
        await Clients.Group($"Event_{eventId}").SendAsync("ReceiveComment", eventId, comment, userService.GetUserIdFromContext());
    }

    
    public async Task JoinEventGroup(Guid eventId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Event_{eventId}");
    }
    
    public async Task LeaveEventGroup(Guid eventId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Event_{eventId}");
    }
}
