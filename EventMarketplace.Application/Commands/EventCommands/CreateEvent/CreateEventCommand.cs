using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Dtos.EventDtos;

namespace EventMarketplace.Application.Commands.EventCommands;

public sealed record CreateEventCommand(CreateEventDto Dto) : ICommand;