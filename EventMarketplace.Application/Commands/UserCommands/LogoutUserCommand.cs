using MediatR;

namespace EventMarketplace.Application.Commands.UserCommands;

public sealed record LogoutUserCommand : IRequest;
