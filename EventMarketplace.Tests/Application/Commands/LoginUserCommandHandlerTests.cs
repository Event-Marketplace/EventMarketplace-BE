using EventMarketplace.Application.Commands.UserCommands;
using EventMarketplace.Application.Commands.UserCommands.Handlers;
using EventMarketplace.Application.Dtos.UserDtos;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
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
        _user = new User()
        {
            EmailAddress = EmailAddress.Create("b.longota2@wp.pl"),
            Password = "Password.122"
        };
    }


    [Fact]
    public async Task Should_Throw_Exception_When_Email_Not_Exist()
    {
        //arrange
        _userRepo.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        
        var command = new LoginUserCommand(
            new LoginUserDto()
            {
                Email = "b.longota2@wp.pl",
                Password = "Password.123"
            });
        
        //act
        Func<Task> action = () => _handler.ExecuteHandleAsync(command, CancellationToken.None);
        
        //asserts
        await Assert.ThrowsAsync<AppException>(action);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Password_Is_Wrong()
    {
        _userRepo.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(_user);
        
        var command = new LoginUserCommand(
            new LoginUserDto()
            {
                Email = "b.longota2@wp.pl",
                Password = "Password.123"
            });

        _passwordManager.Setup(x => x.ValidPassword(command.Dto.Password, _user.Password)).Returns(false);
        
        Func<Task> action = () => _handler.ExecuteHandleAsync(command, CancellationToken.None);

        await Assert.ThrowsAsync<AppException>(action);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}