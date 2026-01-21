using EventMarketplace.Application.CqrsAbstract;
using MediatR;

namespace EventMarketplace.Application.Commands.EventCommands;

public record ReadCommentCommand(Guid EventId) : ITransactionalCommand;
