using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.Commands.EventCommands.AdminFunctions.handlers;

public class RejectEventCommandHandler(
    IUnitOfWork unitOfWork,
    IUserService userService,
    ILogger<RejectEventCommand> logger) : IRequestHandler<RejectEventCommand>
{
    public async Task Handle(RejectEventCommand request, CancellationToken cancellationToken)
    {
        var eventToApprove = await unitOfWork.Events.GetEventByIdAsync(request.EventId) ??
                             throw new AppException("Event not found.");
        
        var currentUserId = userService.GetUserIdFromContext();
        if (!userService.IsInRole(RoleType.Admin)) throw new AppException("User has not specified role.");
        
        if(eventToApprove.EventStatus == EventStatus.Rejected) throw new AppException("Event already rejected!");
        eventToApprove.RejectEvent(request.RejectionReason);
        
        logger.LogInformation($"Event was rejected, event ID: {request.EventId})");
    }
}