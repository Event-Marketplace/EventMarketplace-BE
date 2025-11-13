using EventMarketplace.Application.Abstract;

namespace EventMarketplace.Application.Commands.EventCommands.DeleteEvent;

public sealed record DeleteEventCommand(Guid EventId) : ICommand;