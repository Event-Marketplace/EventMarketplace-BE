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
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IEventRepository> _events = new();
    private readonly SubmitEventCommandHandler _handler;

    public SubmitEventCommandHandlerTests()
    {
        _unitOfWork.SetupGet(x => x.Events).Returns(_events.Object);
        _handler = new SubmitEventCommandHandler(
            _unitOfWork.Object,
            Mock.Of<ILogger<SubmitEventCommandHandler>>());
    }

    [Fact]
    public async Task Handle_DraftEvent_ChangesStatusToSubmitted()
    {
        var eventId = Guid.NewGuid();
        var eventEntity = new Event { Id = eventId, EventStatus = EventStatus.Draft };
        _events.Setup(x => x.GetEventByIdAsync(eventId)).ReturnsAsync(eventEntity);

        await _handler.Handle(new SubmitEventCommand(eventId), CancellationToken.None);

        Assert.Equal(EventStatus.Submitted, eventEntity.EventStatus);
        Assert.NotNull(eventEntity.UpdatedAt);
    }

    [Fact]
    public async Task Handle_AlreadySubmittedEvent_ThrowsConflict()
    {
        var eventId = Guid.NewGuid();
        var eventEntity = new Event { Id = eventId, EventStatus = EventStatus.Submitted };
        _events.Setup(x => x.GetEventByIdAsync(eventId)).ReturnsAsync(eventEntity);

        await Assert.ThrowsAsync<EmConflictException>(() =>
            _handler.Handle(new SubmitEventCommand(eventId), CancellationToken.None));

        Assert.Equal(EventStatus.Submitted, eventEntity.EventStatus);
    }

    [Fact]
    public async Task Handle_EventDoesNotExist_ThrowsNotFound()
    {
        var eventId = Guid.NewGuid();
        _events.Setup(x => x.GetEventByIdAsync(eventId)).ReturnsAsync((Event)null!);

        await Assert.ThrowsAsync<EmNotFoundException>(() =>
            _handler.Handle(new SubmitEventCommand(eventId), CancellationToken.None));
    }
}
