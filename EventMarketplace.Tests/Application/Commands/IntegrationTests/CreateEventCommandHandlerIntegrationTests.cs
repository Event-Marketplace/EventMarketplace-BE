using System.Security.Claims;
using Azure.Storage.Blobs;
using EventMarketplace.Application.Commands.EventCommands;
using EventMarketplace.Application.Commands.EventCommands.Handlers;
using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils.Azure;
using EventMarketplace.Infrastructure.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands.IntegrationTests;

public class CreateEventCommandHandlerIntegrationTests
{
    public CreateEventCommandHandlerIntegrationTests()
    {
        
    }
    
    [Fact]
    public async Task CreateEvent_IntegrationTest()
    {
        //arrange
        var options = new DbContextOptionsBuilder<EventMarketplaceDbContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;

        await using var context = new EventMarketplaceDbContext(options);
        var unitOfWork = new TestUnitOfWork(context);

        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(
            new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "test@o2.pl")
            }, "TestAuth"));

        var accessor = new HttpContextAccessor() { HttpContext = httpContext };
        
        var blob = new Mock<IBlobStorageService>();
        blob.Setup(x => 
            x.UploadFileAsync(
                It.IsAny<IFormFile>(), 
                "events", 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("fake.jpg");

        var logger = new LoggerFactory().CreateLogger<CreateEventCommandHandler>();

        var handler = new CreateEventCommandHandler(accessor, unitOfWork, blob.Object, logger);
        var command = new CreateEventCommand(Dto: new CreateEventDto()
        {
            Title = "Test",
            Description = "Desc",
            Price = 99,
            AvailableTicketsCount = 10,
            StartDateTime = DateTime.UtcNow.AddDays(1),
            EndDateTime = DateTime.UtcNow.AddDays(1).AddHours(2),
            Image = new FormFile(Stream.Null, 0, 0, "file", "file.jpg")
        });
        
        //act
        await handler.Handle(command, CancellationToken.None);

        //asserts
        var savedEvent = await context.Events.FirstOrDefaultAsync();
        Assert.NotNull(savedEvent);
        Assert.Equal("Test", savedEvent.Title);
        Assert.Equal("Desc", savedEvent.Description);
        Assert.Equal(99, savedEvent.Price);
        Assert.Equal(10, savedEvent.AvailableTickets);
        Assert.Equal("fake.jpg", savedEvent.ImageUrl);
    }
}