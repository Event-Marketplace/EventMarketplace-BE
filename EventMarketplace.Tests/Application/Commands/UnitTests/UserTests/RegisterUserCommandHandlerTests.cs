using EventMarketplace.Application.Commands.UserCommands;
using EventMarketplace.Application.Commands.UserCommands.Handlers;
using EventMarketplace.Application.Dtos.UserDtos;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands.UnitTests.UserTests;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<IRoleRepository> _roles = new();
    private readonly Mock<IPasswordManager> _passwords = new();
    private readonly RegisterUserCommandHandler _handler;
    private readonly Role _participantRole = new()
    {
        RoleType = RoleType.Participant,
        DisplayName = "Participant"
    };

    public RegisterUserCommandHandlerTests()
    {
        _unitOfWork.SetupGet(x => x.Users).Returns(_users.Object);
        _unitOfWork.SetupGet(x => x.Roles).Returns(_roles.Object);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _users.Setup(x => x.CheckBusyEmail(It.IsAny<string>())).ReturnsAsync(false);
        _roles.Setup(x => x.GetRoleByEnumAsync(RoleType.Participant)).ReturnsAsync(_participantRole);
        _passwords.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("hashed-password");

        _handler = new RegisterUserCommandHandler(
            _unitOfWork.Object,
            _passwords.Object,
            Mock.Of<ILogger<RegisterUserCommandHandler>>());
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesParticipantAndSaves()
    {
        User? addedUser = null;
        _users.Setup(x => x.AddUserAsync(It.IsAny<User>()))
            .Callback<User>(user => addedUser = user)
            .Returns(Task.CompletedTask);

        await _handler.Handle(CreateCommand(), CancellationToken.None);

        Assert.NotNull(addedUser);
        Assert.Equal("new.user@example.com", addedUser.EmailAddress.Value);
        Assert.Equal("hashed-password", addedUser.Password);
        Assert.Contains(addedUser.UserRoles, userRole => userRole.RoleId == _participantRole.Id);
        _passwords.Verify(x => x.HashPassword("Password.123"), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_EmailAlreadyUsed_ThrowsAndDoesNotSave()
    {
        _users.Setup(x => x.CheckBusyEmail(It.IsAny<string>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<EmException>(() => _handler.Handle(CreateCommand(), CancellationToken.None));

        _users.Verify(x => x.AddUserAsync(It.IsAny<User>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_PasswordsDoNotMatch_ThrowsAndDoesNotSave()
    {
        await Assert.ThrowsAsync<EmException>(() => _handler.Handle(
            CreateCommand(password: "Password.123", confirmPassword: "Different.123"),
            CancellationToken.None));

        _passwords.Verify(x => x.HashPassword(It.IsAny<string>()), Times.Never);
        _users.Verify(x => x.AddUserAsync(It.IsAny<User>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ParticipantRoleMissing_ThrowsAndDoesNotSave()
    {
        _roles.Setup(x => x.GetRoleByEnumAsync(RoleType.Participant)).ReturnsAsync((Role)null!);

        await Assert.ThrowsAsync<EmNotFoundException>(() => _handler.Handle(CreateCommand(), CancellationToken.None));

        _users.Verify(x => x.AddUserAsync(It.IsAny<User>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static RegisterUserCommand CreateCommand(string password = "Password.123", string confirmPassword = "Password.123")
        => new(new RegisterUserDto
        {
            Email = "new.user@example.com",
            Password = password,
            ConfirmPassword = confirmPassword,
            IsOrganizerAccount = false
        });
}
