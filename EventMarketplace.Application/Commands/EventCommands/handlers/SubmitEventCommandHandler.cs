using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.Commands.EventCommands;

public class SubmitEventCommandHandler(IUnitOfWork unitOfWork, ILogger<SubmitEventCommandHandler> logger) : IRequestHandler<SubmitEventCommand>
{
    public async Task Handle(SubmitEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await unitOfWork.Events.GetEventByIdAsync(request.EventId) ??
            throw new AppException("Brak wydarzenia o podanym ID.");

        if (@event.EventStatus == EventStatus.Submitted)
            throw new AppException($"Wydarzenie posiada już status: {@event.EventStatus.GetDisplayName()}");
        
        @event.SubmitEventToAdminVerification();
        logger.LogInformation($"Poprawnie zmieniono status wydarzenia o id: {request.EventId}");
    }
    
}