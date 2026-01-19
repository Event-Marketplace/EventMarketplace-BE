using EventMarketplace.Application.CqrsAbstract;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace EventMarketplace.Application.Commands.EventCommands;

public sealed record AddEventCommentCommand(Guid EventId, string Comment, string CurrentContext) : IRequest<Guid>, ITransactionalCommand;