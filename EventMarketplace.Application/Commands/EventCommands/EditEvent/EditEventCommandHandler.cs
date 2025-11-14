using AutoMapper;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils.Azure;
using MediatR;

namespace EventMarketplace.Application.Commands.EventCommands.EditEvent;

public class EditEventCommandHandler(
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    IBlobStorageService storageService) : IRequestHandler<EditEventCommand>
{
    public async Task Handle(EditEventCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var eventToUdpate = await unitOfWork.Events.GetEventByIdAsync(request.Dto.Id) 
                                ?? throw new AppException("Brak danego wydarzenia w bazie danych.");

            mapper.Map(request.Dto, eventToUdpate);
            
            eventToUdpate.DurationOfTheEvent =
                eventToUdpate.DurationOfTheEvent.Update(request.Dto.StartDateTime, request.Dto.EndDateTime);
            
            if (request.Dto.Image != null)
            {
                var imageStrings = eventToUdpate.ImageUrl.Split('/');
                
                var uri = await storageService.UploadOrReplaceFileAsync(request.Dto.Image, imageStrings.Last(), "events",
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