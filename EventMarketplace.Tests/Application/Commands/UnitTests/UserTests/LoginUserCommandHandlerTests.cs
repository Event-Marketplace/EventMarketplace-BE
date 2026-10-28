using EventMarketplace.Application.Commands.UserCommands;
using EventMarketplace.Application.Commands.UserCommands.Handlers;
using EventMarketplace.Application.Dtos.UserDtos;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Response;
using EventMarketplace.Application.Utils;
using EventMarketplace.Application.Utils.Jwt;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands.UnitTests.UserTests;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<IAuthRepository> _auth = new();
    private readonly Mock<IPasswordManager> _passwords = new();
    private readonly Mock<IJwtProvider> _jwt = new();
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
        _unitOfWork.SetupGet(x => x.Users).Returns(_users.Object);
        _unitOfWork.SetupGet(x => x.Auths).Returns(_auth.Object);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _auth.Setup(x => x.AddNewRefreshTokenAsync(It.IsAny<RefreshToken>()))
            .Returns(Task.CompletedTask);

        _handler = new LoginUserCommandHandler(
            _unitOfWork.Object,
            _passwords.Object,
            Mock.Of<ILogger<LoginUserCommandHandler>>(),
            _jwt.Object);
    }

    [Fact]
    public async Task Handle_UnknownEmail_ThrowsNotFound()
    {
        _users.Setup(x => x.GetUserByEmailAsync("new.user@example.com")).ReturnsAsync((User)null!);

        await Assert.ThrowsAsync<EmNotFoundException>(() => _handler.Handle(CreateCommand(), CancellationToken.None));

        _passwords.Verify(x => x.ValidPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WrongPassword_ThrowsAndDoesNotCreateTokens()
    {
        var user = User.CreateUser("new.user@example.com");
        user.Password = "stored-hash";
        _users.Setup(x => x.GetUserByEmailAsync("new.user@example.com")).ReturnsAsync(user);
        _passwords.Setup(x => x.ValidPassword("Wrong.123", "stored-hash")).Returns(false);

        await Assert.ThrowsAsync<EmException>(() => _handler.Handle(CreateCommand("Wrong.123"), CancellationToken.None));

        _jwt.Verify(x => x.GenerateToken(It.IsAny<User>()), Times.Never);
        _jwt.Verify(x => x.AppendRefreshToken(It.IsAny<string>()), Times.Never);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsJwtAndStoresRefreshToken()
    {
        var user = User.CreateUser("new.user@example.com");
        user.Password = "stored-hash";
        var refreshToken = new RefreshTokenResponse
        {
            RefreshToken = "refresh-token-value",
            Expires = DateTime.UtcNow.AddDays(7)
        };
        _users.Setup(x => x.GetUserByEmailAsync("new.user@example.com")).ReturnsAsync(user);
        _passwords.Setup(x => x.ValidPassword("Password.123", "stored-hash")).Returns(true);
        _jwt.Setup(x => x.GenerateToken(user)).Returns("access-token");
        _jwt.Setup(x => x.GenerateRefreshToken(user)).Returns(refreshToken);

        var result = await _handler.Handle(CreateCommand(), CancellationToken.None);

        Assert.Equal("access-token", result.TokenJwt);
        _jwt.Verify(x => x.AppendRefreshToken("refresh-token-value"), Times.Once);
        _auth.Verify(x => x.AddNewRefreshTokenAsync(It.Is<RefreshToken>(token =>
            token.UserId == user.Id && token.Expires == refreshToken.Expires && !token.Revoked)), Times.Once);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static LoginUserCommand CreateCommand(string password = "Password.123")
        => new(new LoginUserDto { Email = "new.user@example.com", Password = password });
}
