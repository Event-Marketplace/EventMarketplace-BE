
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
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
        if (await unitOfWork.Users.CheckBusyEmail(request.Dto.Email))
            throw new AppException("This email is already busy.");
        
        var memberRole = await unitOfWork.Roles.GetRoleByEnumAsync(RoleType.Participant) 
                         ?? throw new AppException($"Role: {RoleType.Participant.GetDisplayName()} not found.");
        var newUser = User.CreateUser(request.Dto.Email);
        newUser.AssignRole(memberRole);

        if (!request.Dto.Password.Equals(request.Dto.ConfirmPassword))
            throw new AppException("Given passwords are not the same.");

        newUser.SetPassword(passwordManager.HashPassword(request.Dto.Password));
        
        await unitOfWork.Users.AddUserAsync(newUser);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.LogInformation($"Registered successfully user - {newUser.EmailAddress.Value}");
    }
}