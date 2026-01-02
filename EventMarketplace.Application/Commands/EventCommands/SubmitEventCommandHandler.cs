using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.Commands.EventCommands;

public class SubmitEventCommandHandler(IUnitOfWork unitOfWork, ILogger<SubmitEventCommandHandler> logger) : IRequestHandler<SubmitEventCommand>
{
    public async Task Handle(SubmitEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await unitOfWork.Events.GetEventByIdAsync(request.EventId) ??
            throw new AppException("Brak wydarzenia o podanym ID.");
        
        @event.SubmitEventToAdminVerification();
        logger.LogInformation($"Poprawnie zmieniono status wydarzenia o id: {request.EventId}");
    }
}