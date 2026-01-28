using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.Commands.EventCommands.AdminUseCases.handlers;

public class ApproveEventCommandHandler(IUnitOfWork unitOfWork, ILogger<ApproveEventCommandHandler> logger) : IRequestHandler<ApproveEventCommand>
{
    public async Task Handle(ApproveEventCommand request, CancellationToken cancellationToken)
    {
        var eventToApprove = await unitOfWork.Events.GetEventByIdAsync(request.EventId) ??
                             throw new EmNotFoundException("Event not found.");

        if(eventToApprove.EventStatus == EventStatus.Approved) throw new EmConflictException("Event already approved!","EVENT_ALREADY_APPROVED");
        eventToApprove.ApproveEvent();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            $"Zmieniono status z {EventStatus.Submitted.GetDisplayName()} na {EventStatus.Approved.GetDisplayName()}. Wydarzenie o id: {request.EventId})");
    }
}