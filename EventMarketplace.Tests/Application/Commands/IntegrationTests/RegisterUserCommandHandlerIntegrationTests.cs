using EventMarketplace.Application.Commands.UserCommands;
using EventMarketplace.Application.Commands.UserCommands.Handlers;
using EventMarketplace.Application.Dtos.UserDtos;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands.IntegrationTests;

public class RegisterUserCommandHandlerIntegrationTests
{
    [Fact]
    public async Task RegisterUser_IntegrationTest()
    {
        //arrange
        var options = new DbContextOptionsBuilder<EventMarketplaceDbContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;

        await using var context = new EventMarketplaceDbContext(options);

        context.Roles.Add(new Role()
        {
            Id = Guid.CreateVersion7(),
            RoleType = RoleType.Member,
            DisplayName = RoleType.Member.GetDisplayName()
        });

        await context.SaveChangesAsync();
        
        var unitOfWork = new TestUnitOfWork(context);
        var passwordManager = new PasswordManager();
        var logger = new LoggerFactory().CreateLogger<RegisterUserCommandHandler>();
        var handler = new RegisterUserCommandHandler(unitOfWork, passwordManager, logger);

        var command = new RegisterUserCommand(new RegisterUserDto()
        {
            Email = "test@wp.pl",
            Password = "Password.123",
            ConfirmPassword = "Password.123",
            IsOrganizerAccount = false
        });

        //act
        await handler.Handle(command, CancellationToken.None);
        
        //asserts
        var userInDb = await context.Users.FirstOrDefaultAsync(x => x.EmailAddress.Value.Equals("test@wp.pl"));
        Assert.NotNull(userInDb);
    }
}