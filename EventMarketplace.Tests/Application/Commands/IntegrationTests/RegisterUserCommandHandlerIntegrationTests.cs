using EventMarketplace.Application.Commands.UserCommands;
using EventMarketplace.Application.Commands.UserCommands.Handlers;
using EventMarketplace.Application.Dtos.UserDtos;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands.IntegrationTests;

public class RegisterUserCommandHandlerIntegrationTests
{
    [Fact]
    public async Task Handle_ValidRegistration_PersistsUserWithParticipantRoleAndHashedPassword()
    {
        var options = new DbContextOptionsBuilder<EventMarketplaceDbContext>()
            .UseInMemoryDatabase($"RegisterUser_{Guid.NewGuid()}")
            .Options;

        await using var context = new EventMarketplaceDbContext(options);
        var participantRole = new Role
        {
            RoleType = RoleType.Participant,
            DisplayName = RoleType.Participant.GetDisplayName()
        };
        context.Roles.Add(participantRole);
        await context.SaveChangesAsync();

        var passwordManager = new PasswordManager();
        var handler = new RegisterUserCommandHandler(
            new TestUnitOfWork(context),
            passwordManager,
            MockLogger());
        var command = new RegisterUserCommand(new RegisterUserDto
        {
            Email = "integration.user@example.com",
            Password = "Password.123!",
            ConfirmPassword = "Password.123!",
            IsOrganizerAccount = false
        });

        await handler.Handle(command, CancellationToken.None);

        var registeredUser = await context.Users
            .Include(user => user.UserRoles)
            .SingleAsync(user => user.EmailAddress.Value == "integration.user@example.com");

        Assert.NotEqual("Password.123!", registeredUser.Password);
        Assert.True(passwordManager.ValidPassword("Password.123!", registeredUser.Password));
        Assert.Contains(registeredUser.UserRoles, userRole => userRole.RoleId == participantRole.Id);
    }

    private static ILogger<RegisterUserCommandHandler> MockLogger()
        => Microsoft.Extensions.Logging.Abstractions.NullLogger<RegisterUserCommandHandler>.Instance;
}
