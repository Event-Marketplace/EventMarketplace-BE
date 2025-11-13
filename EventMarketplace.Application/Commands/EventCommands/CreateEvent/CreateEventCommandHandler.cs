using System.Security.Claims;
using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils.Azure;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using EventMarketplace.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventMarketplace.Application.Commands.EventCommands.Handlers;

public class CreateEventCommandHandler(
    IHttpContextAccessor contextAccessor,
    IUnitOfWork unitOfWork,
    IBlobStorageService blobStorageService,
    ILogger<CreateEventCommandHandler> logger) : ICommandHandler<CreateEventCommand>
{
    public async Task ExecuteHandleAsync(CreateEventCommand command, CancellationToken cancellationToken = default)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var loggedOrganizer = contextAccessor.HttpContext?.User;
            
            if (loggedOrganizer?.Identity?.IsAuthenticated != true)
                throw new AppException("Używtkownik nie jest zalogowany.");
            
            logger.LogInformation($"Rozpoczęcie procesu dodania nowego wydarzenia przez - {loggedOrganizer.FindFirst(ClaimTypes.Email)}");

            var uploadAndReturnUriFromAzure =
                await blobStorageService.UploadFileAsync(command.Dto.Image, "events", cancellationToken);
            
            var newEvent = new Event()
            {
                Id = Guid.CreateVersion7(),
                Title = command.Dto.Title,
                Description = command.Dto.Description,
                Price = command.Dto.Price,
                AvailableTickets = command.Dto.AvailableTicketsCount,
                DurationOfTheEvent = DurationOfTheEvent.Create(command.Dto.StartDateTime, command.Dto.EndDateTime),
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