using EventMarketplace.Application.Abstract;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.ValueObjects;

namespace EventMarketplace.Application.Commands.UserCommands.Handlers;

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    public async Task ExecuteHandleAsync(RegisterUserCommand command)
    {
        var email = EmailAddress.Create(command.Dto.Email);

        var newUser = new User()
        {
            EmailAddress = email,
            Password = command.Dto.Password,
            CreateAt = DateTime.UtcNow,
            IsOrganizerAccount = false
        };
        
        
    }
}