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
using EventMarketplace.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepo;
    private readonly Mock<IPasswordManager> _passwordManager;
    private readonly Mock<ILogger<LoginUserCommandHandler>> _logger;
    private readonly Mock<IJwtProvider> _jwtProvider;
    private readonly LoginUserCommandHandler _handler;
    private readonly User _user;
    
    public LoginUserCommandHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _userRepo = new Mock<IUserRepository>();
        _passwordManager = new Mock<IPasswordManager>();
        _logger = new Mock<ILogger<LoginUserCommandHandler>>();
        _jwtProvider = new Mock<IJwtProvider>();
        
        _handler = new LoginUserCommandHandler(_unitOfWork.Object, _passwordManager.Object, _logger.Object, _jwtProvider.Object);
        _unitOfWork.Setup(x => x.Users).Returns(_userRepo.Object);
        _unitOfWork.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _user = new User();
    }

    #region NegativeTests

    [Fact]
    public async Task Should_Throw_Exception_When_Email_Not_Exist()
    {
        //arrange
        _userRepo.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        
        var command = new LoginUserCommand(
            new LoginUserDto()
            {
                Email = "b.longota2@wp.pl",
                Password = "Password.122"
            });
        
        //act
        Func<Task> action = () => _handler.Handle(command, CancellationToken.None);
        
        //asserts
        await Assert.ThrowsAsync<AppException>(action);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Password_Is_Wrong()
    {
        _user.Password = "Password.122";
        _userRepo.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(_user);
        
        var command = new LoginUserCommand(
            new LoginUserDto()
            {
                Email = "b.longota2@wp.pl",
                Password = "Password.123"
            });

        _passwordManager.Setup(x => x.ValidPassword(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string plain, string hashed) => plain == hashed);
        
        Func<Task> action = () => _handler.Handle(command, CancellationToken.None);

        await Assert.ThrowsAsync<AppException>(action);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _passwordManager.Verify(x => x.ValidPassword(command.Dto.Password, _user.Password), Times.Once);
    }

    #endregion

    #region PositiveTests

    // [Theory]
    // [InlineData("b.longota2@wp.pl", "Password.123", "qwertyuiop")]
    // [InlineData("b.longota2@wp.pl", "Password.123", "qwertyuio")]
    // public async Task Login_Successfully_When_Valid_Credentials(string email, string password, string token)
    // {
    //     //arrange
    //     _user.Password = password;
    //     _user.EmailAddress = EmailAddress.Create("b.longota2@wp.pl");
    //     _userRepo.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(_user);
    //     _passwordManager.Setup(x => x.ValidPassword(It.IsAny<string>(), It.IsAny<string>()))
    //         .Returns((string p1, string p2) => p1 == p2);
    //     _jwtProvider.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns(token);
    //
    //     var command = new LoginUserCommand(
    //         new LoginUserDto()
    //         {
    //             Email = email,
    //             Password = password
    //         });
    //     //act
    //     var result = await  _handler.Handle(command, CancellationToken.None);
    //     
    //     //asserts
    //     Assert.Equal(token, result.TokenJwt);
    //     _unitOfWork.Verify(x => x.CommitAsync(CancellationToken.None), Times.Once);
    //     _unitOfWork.Verify(x => x.RollbackAsync(CancellationToken.None), Times.Never);
    // }

    #endregion
}