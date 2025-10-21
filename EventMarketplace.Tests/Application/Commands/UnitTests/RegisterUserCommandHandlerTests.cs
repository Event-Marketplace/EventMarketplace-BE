using EventMarketplace.Application.Commands.UserCommands;
using EventMarketplace.Application.Commands.UserCommands.Handlers;
using EventMarketplace.Application.Dtos.UserDtos;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using EventMarketplace.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IUserRepository> _userRepo;
    private readonly Mock<IPasswordManager> _passwordManager;
    private readonly Mock<ILogger<RegisterUserCommandHandler>> _logger;
    private readonly RegisterUserCommandHandler _handler;
    private readonly User _user;
    
    public RegisterUserCommandHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _userRepo = new Mock<IUserRepository>();
        _passwordManager = new Mock<IPasswordManager>();
        _logger = new Mock<ILogger<RegisterUserCommandHandler>>();
        
        _unitOfWork.Setup(u => u.Users).Returns(_userRepo.Object);
        _unitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.RollbackAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        
        _handler = new RegisterUserCommandHandler(_unitOfWork.Object, _passwordManager.Object, _logger.Object);
        _user = new User();
    }

    #region NegativeTests

    [Fact]
    public async void ExecuteHandleAsync_WithValidCommand_ShouldRegisterUser()
    {
        _userRepo.Setup(r => r.CheckBusyEmail(It.IsAny<string>())).ReturnsAsync(true);
        
        var command = new RegisterUserCommand(
            new RegisterUserDto
            {
                Email = "b.longota@op.pl",
                Password = "Password.123",
                ConfirmPassword = "Password.123",
                IsOrganizerAccount = false
            }
        );
        
        //act
        Func<Task> action = () =>  _handler.ExecuteHandleAsync(command, CancellationToken.None);

        //assert
        await Assert.ThrowsAsync<AppException>(action);
        _unitOfWork.Verify(u => u.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Should_Throw_When_Passwords_Not_Same()
    {
        //arrange
        _passwordManager.Setup(m => m.ValidPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
        
        var command = new RegisterUserCommand(
            new RegisterUserDto()
            {
                Email = "b.longota@op.pl",
                Password = "Password.123",
                ConfirmPassword = "Passwordc.123",
                IsOrganizerAccount = false
            }
        );
        //act
        Func<Task> action = () => _handler.ExecuteHandleAsync(command, CancellationToken.None);

        //asserts
        await Assert.ThrowsAsync<AppException>(action);
        _unitOfWork.Verify(m => m.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion

    #region PositiveTests

    [Fact]
    public async Task Register_Successfully_When_Valid_Credentials()
    {
        _userRepo.Setup(x => x.CheckBusyEmail(It.IsAny<string>())).ReturnsAsync(false);
        _passwordManager.Setup(x => x.HashPassword(It.IsAny<string>())).Returns("password");

        var command = new RegisterUserCommand(
            new RegisterUserDto()
            {
                Email = "b.longota2@wp.pl",
                Password = "password",
                ConfirmPassword = "password",
                IsOrganizerAccount = false
            });

        await _handler.ExecuteHandleAsync(command, CancellationToken.None);
        
        _userRepo.Verify(x => x.AddUserAsync(It.IsAny<User>()), Times.Once);
        _passwordManager.Verify(x => x.HashPassword("password"), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(CancellationToken.None),Times.Once);
        _unitOfWork.Verify(x => x.RollbackAsync(CancellationToken.None),Times.Never);

    }

    #endregion
    
}