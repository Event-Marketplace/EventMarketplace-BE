using System.Security.Claims;
using EventMarketplace.Application.Commands.EventCommands.CreateEvent;
using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Application.Utils.Azure;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.Commands.EventCommands.Handlers;

public class CreateEventCommandHandler(
    IUnitOfWork unitOfWork,
    IUserService userService,
    EventFileIUploader eventFileIUploader,
    ILogger<CreateEventCommandHandler> logger) : IRequestHandler<CreateEventCommand>
{
    public async Task Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var loggedOrganizerId = userService.GetUserIdFromContext();
        
        logger.LogInformation($"Rozpoczęcie procesu dodania nowego wydarzenia przez - {userService.GetUserEmailFromContext()}");

        var imageUrl = await eventFileIUploader.UploadEventImageAsync(request.Dto.Image, cancellationToken);
        if (string.IsNullOrEmpty(imageUrl))
            throw new AppException("Upload filed (Azure Blob Storage), Image url is empty!");
        
        var (address, descriptionPlace) = EventAddressMapper.MapLocationToEntity(request.Dto);
        
        var newEvent = Event.Create(request.Dto.Title, request.Dto.Description, request.Dto.Price,
            request.Dto.AvailableTicketsCount, request.Dto.StartDateTime,
            request.Dto.EndDateTime, loggedOrganizerId,
            imageUrl, address, descriptionPlace, request.Dto.LocationType);

        await unitOfWork.Events.AddEventAsync(newEvent, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        logger.LogInformation($"Utworzono wydarzenie o ID - {newEvent.Id}");
    
    }
}