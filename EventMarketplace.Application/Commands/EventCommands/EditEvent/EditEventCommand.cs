
using EventMarketplace.Application.Dtos.EventDtos;
using MediatR;

namespace EventMarketplace.Application.Commands.EventCommands.EditEvent;

public sealed record EditEventCommand(EditEventDto Dto) : IRequest;