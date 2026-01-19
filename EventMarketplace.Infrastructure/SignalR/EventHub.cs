using EventMarketplace.Application.Commands.EventCommands;
using EventMarketplace.Application.Commands.EventCommands.AdminFunctions;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EventMarketplace.Application.Utils.SignalR;

[Authorize(Roles = "Admin,Organizer")]
public class EventHub(IMediator mediator, IUserService userService, IEventCommentRepository eventCommentRepository) : Hub
{
    public async Task AddComment(Guid eventId, string comment, string currentContext)
    {
        var lastCommentId = await mediator.Send<Guid>(new AddEventCommentCommand(eventId, comment, currentContext));

        var currentUserId = userService.GetUserIdFromContext();
        var latestComment = await eventCommentRepository.GetEventCommentByIdAsync(lastCommentId);
        
        var newComment = new EventCommentResponse()
        {
            Id = latestComment.Id,
            Content = latestComment.Content,
            User = latestComment.User.FullName.ToString(),
            CreatedAt = latestComment.CreatedAt.ToLocalTime().ToString("dd.MM.yyyy hh:ss"),
            EventId = latestComment.EventId,
            UserId = latestComment.UserId,
            WasRead = true
        };
        
        await Clients.Group($"Event_{eventId}").SendAsync("ReceiveComment", eventId, newComment);
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
        await mediator.Send(new ReadCommentCommand(eventId));
        await Clients.Group($"Event_{eventId}").SendAsync("CommentsRead", eventId);
    }
}
