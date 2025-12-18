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
                string imageName;
                
                if (!string.IsNullOrWhiteSpace(eventToUdpate.ImageUrl))
                {
                    try
                    {
                        var uri = new Uri(eventToUdpate.ImageUrl);
                        imageName = Path.GetFileName(uri.AbsolutePath);
                    }
                    catch
                    {
                        // fallback: generuj nową nazwę
                        imageName = $"{Guid.NewGuid()}{Path.GetExtension(request.Dto.Image.FileName)}";
                    }
                }
                else
                {
                    imageName = $"{Guid.NewGuid()}{Path.GetExtension(request.Dto.Image.FileName)}";
                }
                
                var newUri = await storageService.UploadOrReplaceFileAsync(request.Dto.Image, imageName, "events",
                    cancellationToken);
                
                eventToUdpate.ImageUrl = newUri;

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