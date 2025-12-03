using System.Security.Claims;
using EventMarketplace.Application.Commands.EventCommands.CreateEvent;
using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils.Azure;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.Commands.EventCommands.Handlers;

public class CreateEventCommandHandler(
    IHttpContextAccessor contextAccessor,
    IUnitOfWork unitOfWork,
    IBlobStorageService blobStorageService,
    ILogger<CreateEventCommandHandler> logger) : IRequestHandler<CreateEventCommand>
{
    public async Task Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var loggedOrganizer = contextAccessor.HttpContext?.User;
            
            if (loggedOrganizer?.Identity?.IsAuthenticated != true)
                throw new AppException("Używtkownik nie jest zalogowany.");
            
            logger.LogInformation($"Rozpoczęcie procesu dodania nowego wydarzenia przez - {loggedOrganizer.FindFirst(ClaimTypes.Email)}");

            var uploadAndReturnUriFromAzure =
                await blobStorageService.UploadFileAsync(request.Dto.Image, "events", cancellationToken);

            var (address, descriptionPlace) = EventAddressMapper.MapLocationToEntity(request.Dto);
            
            var newEvent = Event.Create(request.Dto.Title, request.Dto.Description, request.Dto.Price,
                request.Dto.AvailableTicketsCount, request.Dto.StartDateTime,
                request.Dto.EndDateTime, Guid.Parse(loggedOrganizer.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                uploadAndReturnUriFromAzure, address, descriptionPlace, request.Dto.LocationType);

            await unitOfWork.Events.AddEventAsync(newEvent, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            
            logger.LogInformation($"Utworzono wydarzenie o ID - {newEvent.Id}");
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            logger.LogError($"Błąd podczas dodawania nowego wydarzenia - {e.Message}");
            throw;
        }
    }
}