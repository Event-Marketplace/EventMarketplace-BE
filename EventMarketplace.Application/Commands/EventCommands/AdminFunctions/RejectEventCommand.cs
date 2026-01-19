using EventMarketplace.Application.CqrsAbstract;
using Swashbuckle.AspNetCore.Annotations;

namespace EventMarketplace.Application.Commands.EventCommands.AdminFunctions;

public record RejectEventCommand([SwaggerIgnore] Guid EventId, string RejectionReason) : ICommand;