using EventMarketplace.Application.Commands.EventCommands.AdminUseCases;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.Commands.EventCommands.AdminUseCases.handlers;

public class RejectEventCommandHandler(
    IUnitOfWork unitOfWork,
    IUserService userService,
    ILogger<RejectEventCommand> logger) : IRequestHandler<RejectEventCommand>
{
    public async Task Handle(RejectEventCommand request, CancellationToken cancellationToken)
    {
        var eventToApprove = await unitOfWork.Events.GetEventByIdAsync(request.EventId) ??
                             throw new EmNotFoundException("Event not found.");
        
        var currentUserId = userService.GetUserIdFromContext();
        if (!userService.IsInRole(RoleType.Admin)) throw new EmForbiddenException("User has not specified role.");
        
        if(eventToApprove.EventStatus == EventStatus.Rejected) throw new EmConflictException("Event already rejected!", "EVENT_ALREADY_REJECTED");
        eventToApprove.RejectEvent(request.RejectionReason);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation($"Event was rejected, event ID: {request.EventId})");
    }
}