using EventMarketplace.Application.Commands.EventCommands.DeleteEvent;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using FluentAssertions;
using FluentAssertions.Specialized;
using Moq;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands.UnitTests.EventTests;

public class DeleteEventCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IEventRepository> _eventRepo;
    private readonly DeleteEventCommandHandler _handler;
    
    public DeleteEventCommandHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _eventRepo = new Mock<IEventRepository>();

        _unitOfWork.Setup(x => x.Events).Returns(_eventRepo.Object);
        //_handler = new DeleteEventCommandHandler(_unitOfWork.Object);
    }


    // [Fact]
    // public async Task Success_When_Event_Exist()
    // {
    //     _unitOfWork.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
    //     _unitOfWork.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
    //
    //     _eventRepo.Setup(x => x.DeleteEventAsync(It.IsAny<Event>()));
    //     
    //     var eventToDelete = new Event();
    //     _unitOfWork.Setup(x => x.Events.GetEventByIdAsync(It.IsAny<Guid>())).ReturnsAsync(eventToDelete);
    //
    //     var id = Guid.NewGuid();
    //     var command = new DeleteEventCommand(id);
    //
    //     await _handler.Handle(command, CancellationToken.None);
    //     
    //     _unitOfWork.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    //     _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    //     _unitOfWork.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);
    //     _eventRepo.Verify(x => x.GetEventByIdAsync(id),Times.Once);
    //     _eventRepo.Verify(x => x.DeleteEventAsync(eventToDelete), Times.Once);
    // }
    //
    // [Fact]
    // public async Task Should_Throw_When_Event_Not_Exist()
    // {
    //     _eventRepo.Setup(x => x.GetEventByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Event?)null);
    //
    //     var command = new DeleteEventCommand(Guid.NewGuid());
    //     
    //     // Func<Task> action = () => _handler.Handle(command, CancellationToken.None);
    //     // await Assert.ThrowsAsync<AppException>(action);
    //
    //     await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
    //         .Should()
    //         .ThrowAsync<AppException>();
    //     
    //     _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()),Times.Never);
    //     _unitOfWork.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()),Times.Once);
    // }
}