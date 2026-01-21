using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace EventMarketplace.Application.Commands.EventCommands.AdminUseCases;

public record ApproveEventCommand([SwaggerIgnore] Guid EventId) : IRequest;