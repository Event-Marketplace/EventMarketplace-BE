using AutoMapper;
using EventMarketplace.Application.Commands.EventCommands.CreateEvent;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Services.Events.CreateEvent;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.UseCases.Events.CreateEvent;

public class EventManagementService(
    IUnitOfWork unitOfWork, 
    ILogger<EventManagementService> logger,
    IUserService userService,
    IMapper mapper,
    EventFileUploader eventFileUploader) : IEventManagementService
{
    public async Task CreateEvent(CreateEventRequest request, CancellationToken cancellationToken)
    {
        var loggedOrganizerId = userService.GetUserIdFromContext();
        
        logger.LogInformation($"Rozpoczęcie procesu dodania nowego wydarzenia przez - {userService.GetUserEmailFromContext()}");

        var imageUrl = await eventFileUploader.UploadEventImageAsync(request.Image, cancellationToken);
        if (string.IsNullOrEmpty(imageUrl))
            throw new AppException("Upload filed (Azure Blob Storage), Image url is empty!");
        
        var (address, descriptionPlace) = EventAddressMapper.MapLocationToEntity(request);
        
        var newEvent = Event.Create(request.Title, request.Description, request.Price,
            request.AvailableTicketsCount, request.StartDateTime,
            request.EndDateTime, loggedOrganizerId,
            imageUrl, address, descriptionPlace, request.LocationType);

        await unitOfWork.Events.AddEventAsync(newEvent, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation($"Utworzono wydarzenie o ID - {newEvent.Id}");
    }

    public async Task DeleteEvent(Guid eventId, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var eventToDelete = await unitOfWork.Events.GetEventByIdAsync(eventId) 
                                ?? throw new AppException("Brak wydarzenia o podanym id w bazie danych.");

            if (eventToDelete.EventStatus == EventStatus.Approved) throw new AppException("Nie można usunąć zatwierdzonego wydarzenia.");
            await eventFileUploader.DeleteEventImageAsync(eventToDelete.ImageUrl);
            eventToDelete.IsDeleted = true;
            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            logger.LogError($"Delete image failed!, Event - {eventId}, error: {e.Message}");
            throw;
        }
    }

    public async Task EditEvent(EditEventRequest request, CancellationToken cancellationToken)
    {
        var eventToUdpate = await unitOfWork.Events.GetEventByIdAsync(request.Id) 
                            ?? throw new AppException("Brak danego wydarzenia w bazie danych.");

        mapper.Map(request, eventToUdpate);
        
        eventToUdpate.DurationOfTheEvent =
            eventToUdpate.DurationOfTheEvent.Update(request.StartDateTime, request.EndDateTime);
        
        if (request.Image != null)
        {
            var newUri = await eventFileUploader.UploadOrReplaceFileAsync(eventToUdpate.ImageUrl, request.Image, cancellationToken);
            eventToUdpate.ImageUrl = newUri;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation($"Event update successfully, event - {eventToUdpate.Id}");
    }
}