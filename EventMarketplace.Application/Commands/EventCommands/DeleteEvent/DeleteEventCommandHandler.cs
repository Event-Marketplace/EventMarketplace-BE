using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Domain.Repositories;

namespace EventMarketplace.Application.Commands.EventCommands.DeleteEvent;

public class DeleteEventCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteEventCommand>
{
    public async Task ExecuteHandleAsync(DeleteEventCommand command, CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var eventToDelete = await unitOfWork.Events.GetEventByIdAsync(command.EventId) 
                                ?? throw new AppException("Brak wydarzenia o podanym id w bazie danych.");
            
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