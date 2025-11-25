using System.Security.Claims;
using Castle.Core.Logging;
using EventMarketplace.Application.Commands.EventCommands;
using EventMarketplace.Application.Commands.EventCommands.Handlers;
using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils.Azure;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands.UnitTests.EventTests;

public class CreateEventCommandHandlerTests
{
    //tworzymy mocki (puste / sztuczne obiekty)
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IBlobStorageService> _blobStorageService;
    private readonly Mock<ILogger<CreateEventCommandHandler>> _logger;
    private readonly CreateEventCommandHandler _handler;
    private readonly Mock<IEventRepository> _eventRepo;

    public CreateEventCommandHandlerTests()
    {
        //inicjalizujemy mocki, dzięki temu każdy ma metody (.Setup() - ustaw zachowanie, .Returns() - ustaw co ma zwracać)
        _eventRepo = new Mock<IEventRepository>();
        _httpContextAccessor = new Mock<IHttpContextAccessor>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _blobStorageService = new Mock<IBlobStorageService>();
        _logger = new Mock<ILogger<CreateEventCommandHandler>>();
        
        //tworzymy handler z zależnościami zmockowanymi
        _handler = new CreateEventCommandHandler(_httpContextAccessor.Object, _unitOfWork.Object,
            _blobStorageService.Object, _logger.Object);
        
        //ustalamy zachowania mocków (podstawowe działanie unit of work)
        _unitOfWork.Setup(x => x.Events).Returns(_eventRepo.Object); //kiedy handler odwoła się do unitOfWork.Events = zwróć mu atrapę repo
        //udawaj że transakcja została otwarta
        _unitOfWork.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        //udawaj że commit się udał
        _unitOfWork.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
    }
    
    [Fact]
    public async Task CreatedSuccess_WhenData_IsValid()
    {
        //arrange
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, "test@o2.pl")
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
            
        var context = new DefaultHttpContext();
        context.User = user;

        _httpContextAccessor.Setup(x => x.HttpContext).Returns(context);
        
        _blobStorageService.Setup(x =>
            x.UploadFileAsync(
                It.IsAny<IFormFile>(), 
                It.IsAny<string>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("fakeurifromazure.jpg");

        _eventRepo.Setup(x => x.AddEventAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var dto = new CreateEventDto()
        {
            Title = "string",
            Description = "description",
            Price = 3.4,
            Image = new FormFile(Stream.Null, 0, 0, "file", "file.jpg"),
            AvailableTicketsCount = 210,
            StartDateTime = DateTime.UtcNow.AddDays(1),
            EndDateTime = DateTime.UtcNow.AddDays(3),
        };

        var command = new CreateEventCommand(dto);

        //act
        await _handler.Handle(command, CancellationToken.None);

        //asserts
        _eventRepo.Verify(x => x.AddEventAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()),Times.Once);
    }

    [Fact]
    public async Task Should_Throw_When_User_Is_Not_Logged_In()
    {   
        //arrange
        _httpContextAccessor.Setup(x => x.HttpContext.User.Identity.IsAuthenticated).Returns(false);
        
        var dto = new CreateEventDto()
        {
            Title = "string",
            Description = "description",
            Price = 3.4,
            Image = new FormFile(Stream.Null, 0, 0, "file", "file.jpg"),
            AvailableTicketsCount = 210,
            StartDateTime = DateTime.UtcNow.AddDays(1),
            EndDateTime = DateTime.UtcNow.AddDays(3),
        };

        var command = new CreateEventCommand(dto);
        
        //act
        Func<Task> action = () => _handler.Handle(command, CancellationToken.None);
        
        //asserts
        await Assert.ThrowsAsync<AppException>(action);
        _unitOfWork.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}