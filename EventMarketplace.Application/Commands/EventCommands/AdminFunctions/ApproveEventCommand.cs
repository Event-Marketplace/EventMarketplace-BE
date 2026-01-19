using EventMarketplace.Application.CqrsAbstract;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace EventMarketplace.Application.Commands.EventCommands.AdminFunctions;

public record ApproveEventCommand([SwaggerIgnore] Guid EventId) : IRequest;