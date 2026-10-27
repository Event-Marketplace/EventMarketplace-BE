using AutoMapper;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using EventMarketplace.Application.Utils.Azure;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.ValueObjects;
using EventMarketplace.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using EventManagementService = EventMarketplace.Application.UseCases.Events.CreateEvent.EventManagementService;

namespace EventMarketplace.Tests.Application.Commands.IntegrationTests;

public class DeleteEventIntegrationTests
{
    [Fact]
    public async Task DeleteEvent_DraftEvent_SoftDeletesAndPersistsTheChange()
    {
        var options = new DbContextOptionsBuilder<EventMarketplaceDbContext>()
            .UseInMemoryDatabase($"DeleteEvent_{Guid.NewGuid()}")
            .Options;

        await using var context = new EventMarketplaceDbContext(options);
        var organizer = User.CreateUser("organizer@example.com");
        organizer.Password = "hashed-password";
        var eventEntity = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Draft event",
            Description = "Event used by an integration test",
            DurationOfTheEvent = DurationOfTheEvent.Create(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(2)),
            Price = 10,
            AvailableTickets = 20,
            ImageUrl = "https://storage.example.com/events/poster.jpg",
            EventStatus = EventStatus.Draft,
            OrganizerId = organizer.Id,
            Organizer = organizer,
            LocationType = LocationType.DescriptionPlace
        };
        context.Events.Add(eventEntity);
        await context.SaveChangesAsync();

        var blobStorage = new Mock<IBlobStorageService>();
        blobStorage.Setup(x => x.RemoveImageFromAzureBlob("poster.jpg", "events"))
            .Returns(Task.CompletedTask);
        var unitOfWork = new TestUnitOfWork(context);
        var service = new EventManagementService(
            unitOfWork,
            Mock.Of<ILogger<EventManagementService>>(),
            Mock.Of<IUserService>(),
            Mock.Of<IMapper>(),
            new EventFileUploader(blobStorage.Object));

        await service.DeleteEvent(eventEntity.Id, CancellationToken.None);

        var persistedEvent = await context.Events.AsNoTracking().SingleAsync(item => item.Id == eventEntity.Id);
        Assert.True(persistedEvent.IsDeleted);
        blobStorage.Verify(x => x.RemoveImageFromAzureBlob("poster.jpg", "events"), Times.Once);
    }
}
