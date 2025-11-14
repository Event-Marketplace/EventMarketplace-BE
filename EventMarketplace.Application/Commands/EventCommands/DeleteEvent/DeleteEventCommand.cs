

using MediatR;

namespace EventMarketplace.Application.Commands.EventCommands.DeleteEvent;

public sealed record DeleteEventCommand(Guid EventId) : IRequest;