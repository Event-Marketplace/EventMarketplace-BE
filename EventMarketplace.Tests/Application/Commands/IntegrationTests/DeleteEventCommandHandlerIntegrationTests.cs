using EventMarketplace.Application.Commands.EventCommands.DeleteEvent;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;
using EventMarketplace.Domain.ValueObjects;
using EventMarketplace.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands.IntegrationTests;

public class DeleteEventCommandHandlerIntegrationTests
{
    //[Fact]
    // public async Task DeleteEvent_IntegrationTest()
    // {
    //     var options = new DbContextOptionsBuilder<EventMarketplaceDbContext>()
    //         .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
    //         .Options;
    //
    //     await using var context = new EventMarketplaceDbContext(options);
    //     
    //     var eventNEw = new Event()
    //     {
    //         Id = Guid.NewGuid(),
    //         Title = "sad",
    //         Description = "asdasd",
    //         AvailableTickets = 100,
    //         DurationOfTheEvent = DurationOfTheEvent.Create(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3)),
    //         Price = 100,
    //         ImageUrl = "asdas",
    //         EventStatus = EventStatus.DeletedByOrganizer,
    //         CreateAt = DateTime.UtcNow
    //     };
    //
    //     await context.Events.AddAsync(eventNEw);
    //     await context.SaveChangesAsync();
    //
    //     var unitOfWork = new TestUnitOfWork(context);
    //     var handler = new DeleteEventCommandHandler(unitOfWork);
    //     var command = new DeleteEventCommand(eventNEw.Id);
    //
    //     await handler.Handle(command, CancellationToken.None);
    //     
    //     var deletedEvent = await context.Events.FindAsync(eventNEw.Id);
    //
    //     Assert.Null(deletedEvent);
    // }
}