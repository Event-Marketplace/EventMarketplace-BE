using EventMarketplace.Application.CqrsAbstract;
using Swashbuckle.AspNetCore.Annotations;

namespace EventMarketplace.Application.Commands.EventCommands;

public sealed record AddEventCommentCommand(Guid EventId, string Content) : ICommand;