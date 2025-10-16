using EventMarketplace.Application.Commands.UserCommands;
using EventMarketplace.Application.Commands.UserCommands.Handlers;
using EventMarketplace.Application.Dtos.UserDtos;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventMarketplace.Tests;


public class UserLogicTest
{

    public UserLogicTest()
    {
            
    }
    
    [Fact]
    public async void ExecuteHandleAsync_WithValidCommand_ShouldRegisterUser()
    {
        //arrange
        var mocUoW = new Mock<IUnitOfWork>();
        var mocUserRepo = new Mock<IUserRepository>();
        var mocPasswordManager = new Mock<IPasswordManager>();
        var mocLogger = new Mock<ILogger<RegisterUserCommandHandler>>();

        mocUoW.Setup(u => u.Users).Returns(mocUserRepo.Object);
        mocUoW.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        mocUoW.Setup(u => u.RollbackAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        mocUserRepo.Setup(r => r.CheckBusyEmail(It.IsAny<string>())).ReturnsAsync(true);

        var handler = new RegisterUserCommandHandler(mocUoW.Object, mocPasswordManager.Object, mocLogger.Object);

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
        await Assert.ThrowsAsync<AppException>(() => handler.ExecuteHandleAsync(command, CancellationToken.None));

        //assert
        
        mocUoW.Verify(u => u.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        mocUoW.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);


    }
}