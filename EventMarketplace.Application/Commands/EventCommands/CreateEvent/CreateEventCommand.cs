
using EventMarketplace.Application.Dtos.EventDtos;
using MediatR;

namespace EventMarketplace.Application.Commands.EventCommands;

public sealed record CreateEventCommand(CreateEventDto Dto) : IRequest;