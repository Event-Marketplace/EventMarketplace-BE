using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Dtos.UserDtos;

namespace EventMarketplace.Application.Commands.UserCommands;

public sealed record LoginUserCommand(LoginUserDto Dto) : ICommand;