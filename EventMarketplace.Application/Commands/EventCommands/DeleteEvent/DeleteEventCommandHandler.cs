
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.Repositories;
using MediatR;

namespace EventMarketplace.Application.Commands.EventCommands.DeleteEvent;

public class DeleteEventCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteEventCommand>
{
    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var eventToDelete = await unitOfWork.Events.GetEventByIdAsync(request.EventId) 
                                ?? throw new AppException("Brak wydarzenia o podanym id w bazie danych.");

            if (eventToDelete.EventStatus == EventStatus.Aproved) throw new AppException("Nie można usunąć zatwierdzonego wydarzenia.");
            
            await unitOfWork.Events.DeleteEventAsync(eventToDelete);
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}