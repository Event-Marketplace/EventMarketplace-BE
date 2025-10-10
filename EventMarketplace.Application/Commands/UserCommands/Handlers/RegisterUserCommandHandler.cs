using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace EventMarketplace.Application.Commands.UserCommands.Handlers;

public sealed class RegisterUserCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<RegisterUserCommand>
{
    public async Task ExecuteHandleAsync(RegisterUserCommand command)
    {
        await unitOfWork.BeginTransactionAsync();

        try
        {
            var email = EmailAddress.Create(command.Dto.Email);
            
            var newUser = new User()
            {
                EmailAddress = email,
                CreateAt = DateTime.UtcNow,
                IsOrganizerAccount = false
            };
            
            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(newUser, command.Dto.Password);
            newUser.Password = hashedPassword;
            
            await unitOfWork.Users.AddUserAsync(newUser);
            await unitOfWork.CommitAsync();
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }
}