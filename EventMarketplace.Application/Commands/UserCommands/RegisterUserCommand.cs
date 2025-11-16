
using EventMarketplace.Application.Dtos.UserDtos;
using MediatR;

namespace EventMarketplace.Application.Commands.UserCommands;

public sealed record RegisterUserCommand(RegisterUserDto Dto) : IRequest;