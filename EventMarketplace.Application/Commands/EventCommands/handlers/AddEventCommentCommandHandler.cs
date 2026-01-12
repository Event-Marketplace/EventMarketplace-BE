using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.Commands.EventCommands;

public class AddEventCommentCommandHandler(
    IUnitOfWork unitOfWork, 
    IUserService userService,
    ILogger<AddEventCommentCommandHandler> logger) : IRequestHandler<AddEventCommentCommand, Guid>
{
    public async Task<Guid> Handle(AddEventCommentCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Comment)) throw new AppException("Content is required.");
        if (!await unitOfWork.Events.CheckIsEventExist(request.EventId))
            throw new AppException($"Event with id = {request.EventId} not found!");

        var currentUserId = userService.GetUserIdFromContext();
        
        if (!userService.IsInRole(RoleType.Admin) && !userService.IsInRole(RoleType.Organizer))
            throw new AppException("No permissions.");

        if (request.CurrentContext.Equals(RoleType.Organizer.ToString()))
        {
            if (!await userService.CanUserAccessEvent(request.EventId, currentUserId))
                throw new AppException("User has not access to this event");
        }

        var newComment = EventComment.Create(request.EventId, currentUserId, request.Comment);
        await unitOfWork.EventComments.AddEventCommentAsync(newComment);
        logger.LogInformation($"New Comment was created successfully! CommentId = {newComment.Id}");

        return newComment.Id;
    }
}