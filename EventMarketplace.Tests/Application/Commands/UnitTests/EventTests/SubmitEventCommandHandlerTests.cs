using EventMarketplace.Application.Commands.EventCommands;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands.UnitTests.EventTests;

public class SubmitEventCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<ILogger<SubmitEventCommandHandler>> _logger;
    private readonly Mock<IEventRepository> _eventRepo;
    private readonly SubmitEventCommandHandler _handler;
    
    public SubmitEventCommandHandlerTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _logger = new Mock<ILogger<SubmitEventCommandHandler>>();
        _eventRepo = new Mock<IEventRepository>();
        _handler = new SubmitEventCommandHandler(_unitOfWork.Object, _logger.Object);
        
        //results remote
        _unitOfWork.Setup(x => x.Events).Returns(_eventRepo.Object);
    }
    
    [Fact]
    public async Task Success_SubmitEvent()
    {
        //arrange
        var newGuid = Guid.CreateVersion7();
        var command = new SubmitEventCommand(newGuid);
        var eventToUdpate = new Event()
        {
            Id = new Guid(),
            EventStatus = EventStatus.Draft
        };

        _unitOfWork.Setup(x => x.Events.GetEventByIdAsync(newGuid)).ReturnsAsync(eventToUdpate);

        //act
        await _handler.Handle(command, CancellationToken.None);

        //asserts
        Assert.Equal(EventStatus.Submitted, eventToUdpate.EventStatus);
        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
    
    [Fact]
    public async Task Handle_Throws_WhenEventAlreadySubmitted()
    {
        //arrange
        var newGuid = Guid.CreateVersion7();
        var command = new SubmitEventCommand(newGuid);
        var eventToUdpate = new Event()
        {
            Id = new Guid(),
            EventStatus = EventStatus.Submitted
        };

        _unitOfWork.Setup(x => x.Events.GetEventByIdAsync(newGuid)).ReturnsAsync(eventToUdpate);

        //act
        var ex = await Assert.ThrowsAsync<EmException>(
            () => _handler.Handle(command, CancellationToken.None));

        //asserts
        Assert.Contains("Wydarzenie posiada już status", ex.Message);
    }
}