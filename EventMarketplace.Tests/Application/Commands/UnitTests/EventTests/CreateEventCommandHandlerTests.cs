using Castle.Core.Logging;
using EventMarketplace.Application.Commands.EventCommands.Handlers;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils.Azure;
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
        
        
        //act
        
    
        //asserts
    }
    
}