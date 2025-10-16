using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Dtos.UserDtos;

namespace EventMarketplace.Application.Commands.UserCommands;

public sealed record RegisterUserCommand(RegisterUserDto Dto) : ICommand;