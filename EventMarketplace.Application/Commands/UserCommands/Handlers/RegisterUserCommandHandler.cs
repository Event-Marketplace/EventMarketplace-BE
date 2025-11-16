
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventMarketplace.Application.Commands.UserCommands.Handlers;

public sealed class RegisterUserCommandHandler(
    IUnitOfWork unitOfWork, 
    IPasswordManager passwordManager,
    ILogger<RegisterUserCommandHandler> logger
    ) : IRequestHandler<RegisterUserCommand>
{
    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            if (await unitOfWork.Users.CheckBusyEmail(request.Dto.Email))
                throw new AppException("Ten adres email jest już zajęty.");
            
            var email = EmailAddress.Create(request.Dto.Email);
            
            var newUser = new User()
            {
                EmailAddress = email,
                CreateAt = DateTime.UtcNow,
                IsOrganizerAccount = request.Dto.IsOrganizerAccount
            };

            if (!request.Dto.Password.Equals(request.Dto.ConfirmPassword))
                throw new AppException("Wprowadzone hasła nie są jednakowe.");
            
            var hashedPassword = passwordManager.HashPassword(request.Dto.Password);
            newUser.Password = hashedPassword;
            
            await unitOfWork.Users.AddUserAsync(newUser);
            await unitOfWork.CommitAsync(cancellationToken);
            logger.LogInformation($"Udana rejestracja użytkownika - {newUser.EmailAddress.Value}");
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}