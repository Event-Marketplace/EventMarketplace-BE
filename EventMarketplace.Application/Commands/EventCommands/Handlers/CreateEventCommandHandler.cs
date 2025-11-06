using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using EventMarketplace.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace EventMarketplace.Application.Commands.EventCommands.Handlers;

public class CreateEventCommandHandler(
    IEventRepository eventRepository,
    IUserRepository userRepository,
    IHttpContextAccessor contextAccessor
    ) : ICommandHandler<CreateEventCommand>
{
    public async Task ExecuteHandleAsync(CreateEventCommand command, CancellationToken cancellationToken = default)
    {
        var loggedOrganizer = contextAccessor.HttpContext.User.Identity;
        
        
        var newEvent = new Event()
        {
            Id = Guid.CreateVersion7(),
            Title = "Wyjazd na mecz El classico z udziałem FC Barcelony oraz Realu Madryt",
            Description = "Wspólny wyjazd na mecz EL CLASSICO! Oferujemy w cenie biletu pobyt w hotelu (1 noc) oraz pełne wyżywienie. Zapraszamy do wspólnej przygody oraz poczucia emocji i doświadczenia na żywo tak wielkiego wydarzenia piłki nożnej.",
            Price = 6500.00,
            AvailableTickets = 50,
            DurationOfTheEvent = DurationOfTheEvent.Create(
                new DateTime(2026, 03, 13, 8, 00, 00, DateTimeKind.Utc), 
                new DateTime(2026, 03, 14, 12, 00, 00, DateTimeKind.Utc)),
            
            
        };  
    }
}