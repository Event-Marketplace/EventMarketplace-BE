using EventMarketplace.Application.Response;
using MediatR;

namespace EventMarketplace.Application.Commands.UserCommands;

public sealed record RegenerateTokensCommand : IRequest<JwtTokenResponse>;