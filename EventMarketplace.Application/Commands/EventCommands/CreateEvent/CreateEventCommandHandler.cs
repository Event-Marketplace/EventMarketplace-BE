using System.Security.Claims;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils.Azure;
using EventMarketplace.Domain.Entities;
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
            
            var newEvent = new Event()
            {
                Id = Guid.CreateVersion7(),
                Title = request.Dto.Title,
                Description = request.Dto.Description,
                Price = request.Dto.Price,
                AvailableTickets = request.Dto.AvailableTicketsCount,
                DurationOfTheEvent = DurationOfTheEvent.Create(request.Dto.StartDateTime, request.Dto.EndDateTime),
                OrganizerId = Guid.Parse(loggedOrganizer.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                ImageUrl = uploadAndReturnUriFromAzure,
                CreateAt = DateTime.UtcNow,
            };

            //problem z datami, w bazie mam infinity przez co ta flaga źle się setuje
            newEvent.IsActive = newEvent.DurationOfTheEvent.StartEvent.Date >= DateTime.UtcNow.Date;

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