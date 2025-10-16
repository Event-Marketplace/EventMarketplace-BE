using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Dtos.UserDtos;
using EventMarketplace.Application.Response;

namespace EventMarketplace.Application.Commands.UserCommands;

public sealed record LoginUserCommand(LoginUserDto Dto) : ICommand<LoginUserResponse>;