using EventMarketplace.Application.CqrsAbstract;
using Swashbuckle.AspNetCore.Annotations;

namespace EventMarketplace.Application.Commands.EventCommands.AdminFunctions;

public record ApproveEventCommand([SwaggerIgnore] Guid EventId) : ICommand;