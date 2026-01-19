using EventMarketplace.Application.CqrsAbstract;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace EventMarketplace.Application.Commands.EventCommands;

public record SubmitEventCommand([SwaggerIgnore] Guid EventId) : ITransactionalCommand;