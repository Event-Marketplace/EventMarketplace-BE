using AutoMapper;
using Azure.Storage.Blobs;
using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils.Azure;
using EventMarketplace.Domain.ValueObjects;

namespace EventMarketplace.Application.Commands.EventCommands.EditEvent;

public class EditEventCommandHandler(
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    IBlobStorageService storageService) : ICommandHandler<EditEventCommand>
{
    public async Task ExecuteHandleAsync(EditEventCommand command, CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var eventToUdpate = await unitOfWork.Events.GetEventByIdAsync(command.Dto.Id) 
                                ?? throw new AppException("Brak danego wydarzenia w bazie danych.");

            mapper.Map(command.Dto, eventToUdpate);
            
            eventToUdpate.DurationOfTheEvent =
                eventToUdpate.DurationOfTheEvent.Update(command.Dto.StartDateTime, command.Dto.EndDateTime);
            
            if (command.Dto.Image != null)
            {
                var imageStrings = eventToUdpate.ImageUrl.Split('/');
                
                var uri = await storageService.UploadOrReplaceFileAsync(command.Dto.Image, imageStrings.Last(), "events",
                    cancellationToken);
                
                eventToUdpate.ImageUrl = uri;

                await unitOfWork.Events.UpdateEventAsync(eventToUdpate);
            }

            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}