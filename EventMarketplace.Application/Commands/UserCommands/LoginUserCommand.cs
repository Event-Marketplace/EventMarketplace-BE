
using EventMarketplace.Application.Dtos.UserDtos;
using EventMarketplace.Application.Response;
using MediatR;

namespace EventMarketplace.Application.Commands.UserCommands;

public sealed record LoginUserCommand(LoginUserDto Dto) : IRequest<LoginUserResponse>;