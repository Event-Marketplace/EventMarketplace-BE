using AutoMapper;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Application.Utils.Azure;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.Repositories;
using EventMarketplace.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using EventManagementService = EventMarketplace.Application.UseCases.Events.CreateEvent.EventManagementService;

namespace EventMarketplace.Tests.Application.Commands.UnitTests.EventTests;

public class DeleteEventServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IEventRepository> _events = new();
    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<IBlobStorageService> _blobStorage = new();
    private readonly EventManagementService _service;

    public DeleteEventServiceTests()
    {
        _unitOfWork.SetupGet(x => x.Events).Returns(_events.Object);
        _unitOfWork.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWork.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWork.Setup(x => x.RollbackAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _blobStorage.Setup(x => x.RemoveImageFromAzureBlob(It.IsAny<string>(), "events"))
            .Returns(Task.CompletedTask);

        _service = new EventManagementService(
            _unitOfWork.Object,
            Mock.Of<ILogger<EventManagementService>>(),
            _userService.Object,
            Mock.Of<IMapper>(),
            new EventFileUploader(_blobStorage.Object));
    }

    [Fact]
    public async Task DeleteEvent_ExistingDraft_MarksEventDeletedAndCommits()
    {
        var eventEntity = CreateEvent(EventStatus.Draft);
        _events.Setup(x => x.GetEventByIdAsync(eventEntity.Id)).ReturnsAsync(eventEntity);

        await _service.DeleteEvent(eventEntity.Id, CancellationToken.None);

        Assert.True(eventEntity.IsDeleted);
        _blobStorage.Verify(x => x.RemoveImageFromAzureBlob("poster.jpg", "events"), Times.Once);
        _unitOfWork.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteEvent_ApprovedEvent_ThrowsConflictAndRollsBack()
    {
        var eventEntity = CreateEvent(EventStatus.Approved);
        _events.Setup(x => x.GetEventByIdAsync(eventEntity.Id)).ReturnsAsync(eventEntity);

        await Assert.ThrowsAsync<EmConflictException>(() =>
            _service.DeleteEvent(eventEntity.Id, CancellationToken.None));

        Assert.False(eventEntity.IsDeleted);
        _blobStorage.Verify(x => x.RemoveImageFromAzureBlob(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEvent_UnknownId_ThrowsNotFoundAndRollsBack()
    {
        var eventId = Guid.NewGuid();
        _events.Setup(x => x.GetEventByIdAsync(eventId)).ReturnsAsync((Event)null!);

        await Assert.ThrowsAsync<EmNotFoundException>(() =>
            _service.DeleteEvent(eventId, CancellationToken.None));

        _blobStorage.Verify(x => x.RemoveImageFromAzureBlob(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static Event CreateEvent(EventStatus status)
        => new()
        {
            Id = Guid.NewGuid(),
            Title = "Test event",
            Description = "Event used by a unit test",
            DurationOfTheEvent = DurationOfTheEvent.Create(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2)),
            Price = 25,
            AvailableTickets = 50,
            ImageUrl = "https://storage.example.com/events/poster.jpg",
            EventStatus = status,
            LocationType = LocationType.DescriptionPlace
        };
}
