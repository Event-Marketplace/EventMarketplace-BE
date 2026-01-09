using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.Commands.EventCommands;

public class AddEventCommentCommandHandler(
    IUnitOfWork unitOfWork, 
    IUserService userService,
    ILogger<AddEventCommentCommandHandler> logger) : IRequestHandler<AddEventCommentCommand>
{
    public async Task Handle(AddEventCommentCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Content)) throw new AppException("Content is required.");
        if (!await unitOfWork.Events.CheckIsEventExist(request.EventId))
            throw new AppException($"Event with id = {request.EventId} not found!");

        var currentUserId = userService.GetUserIdFromContext();

        var newComment = EventComment.Create(request.EventId, currentUserId, request.Content);
        await unitOfWork.EventComments.AddEventCommentAsync(newComment);
        logger.LogInformation($"New Comment was created successfully! CommentId = {newComment.Id}");
    }
}