using EventMarketplace.Application.Commands.EventCommands;
using EventMarketplace.Application.Commands.EventCommands.AdminFunctions;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace EventMarketplace.Application.Utils.SignalR;

public class EventHub(IMediator mediator, IUserService userService, IEventCommentRepository eventCommentRepository) : Hub
{
    public async Task RejectEvent(Guid eventId, string comment)
    {
        await mediator.Send(new RejectEventCommand(eventId, comment));
        await Clients.Group($"Event_{eventId}").SendAsync("RejectedEvent", eventId, comment, userService.GetUserIdFromContext());
    }
    
    public async Task AddComment(Guid eventId, string comment)
    {
        await mediator.Send(new AddEventCommentCommand(eventId, comment));

        var currentUserId = userService.GetUserIdFromContext();
        var latestComment = await eventCommentRepository.GetLastCommentByEventAndUser(eventId, currentUserId);
        
        var newComment = new EventCommentResponse()
        {
            Id = latestComment.Id,
            Content = latestComment.Content,
            User = latestComment.User.FullName.ToString(),
            CreatedAt = latestComment.CreatedAt.ToString("dd.MM.yyyy hh:ss"),
            EventId = latestComment.EventId,
            UserId = latestComment.UserId
        };
        
        await Clients.Group($"Event_{eventId}").SendAsync("ReceiveComment", newComment);
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
