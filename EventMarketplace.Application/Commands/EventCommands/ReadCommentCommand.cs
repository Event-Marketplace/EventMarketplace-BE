using EventMarketplace.Application.CqrsAbstract;

namespace EventMarketplace.Application.Commands.EventCommands;

public record ReadCommentCommand(Guid EventId) : ICommand;
