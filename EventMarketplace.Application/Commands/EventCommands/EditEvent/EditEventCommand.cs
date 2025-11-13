using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Dtos.EventDtos;

namespace EventMarketplace.Application.Commands.EventCommands.EditEvent;

public sealed record EditEventCommand(EditEventDto Dto) : ICommand;