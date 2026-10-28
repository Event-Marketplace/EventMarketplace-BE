using AutoMapper;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Services.Events.CreateEvent;
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

public class EditEventServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IEventRepository> _events = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IBlobStorageService> _blobStorage = new();
    private readonly EventManagementService _service;

    public EditEventServiceTests()
    {
        _unitOfWork.SetupGet(x => x.Events).Returns(_events.Object);
        _unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _service = new EventManagementService(
            _unitOfWork.Object,
            Mock.Of<ILogger<EventManagementService>>(),
            Mock.Of<IUserService>(),
            _mapper.Object,
            new EventFileUploader(_blobStorage.Object));
    }

    [Fact]
    public async Task EditEvent_ExistingEvent_MapsChangesAndSaves()
    {
        var eventEntity = CreateEvent();
        var start = DateTime.UtcNow.AddDays(3);
        var end = DateTime.UtcNow.AddDays(4);
        var request = new EditEventRequest
        {
            Id = eventEntity.Id,
            Title = "Updated title",
            StartDateTime = start,
            EndDateTime = end
        };
        _events.Setup(x => x.GetEventByIdAsync(eventEntity.Id)).ReturnsAsync(eventEntity);

        await _service.EditEvent(request, CancellationToken.None);

        Assert.Equal("Updated title", eventEntity.Title);
        Assert.Equal(start, eventEntity.DurationOfTheEvent.StartEvent);
        Assert.Equal(end, eventEntity.DurationOfTheEvent.EndEvent);
        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task EditEvent_UnknownId_ThrowsNotFoundAndDoesNotSave()
    {
        var eventId = Guid.NewGuid();
        _events.Setup(x => x.GetEventByIdAsync(eventId)).ReturnsAsync((Event)null!);

        await Assert.ThrowsAsync<EmNotFoundException>(() =>
            _service.EditEvent(new EditEventRequest { Id = eventId }, CancellationToken.None));

        _unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static Event CreateEvent()
        => new()
        {
            Id = Guid.NewGuid(),
            Title = "Original title",
            Description = "Event used by a unit test",
            DurationOfTheEvent = DurationOfTheEvent.Create(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2)),
            Price = 25,
            AvailableTickets = 50,
            ImageUrl = "https://storage.example.com/events/poster.jpg",
            EventStatus = EventStatus.Draft,
            LocationType = LocationType.DescriptionPlace
        };
}
