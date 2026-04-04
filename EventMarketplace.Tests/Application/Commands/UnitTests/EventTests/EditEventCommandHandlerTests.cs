using AutoMapper;
//using EventMarketplace.Application.Commands.EventCommands.EditEvent;
using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Application.Mapper;
using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils.Azure;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Repositories;
using EventMarketplace.Domain.ValueObjects;
using EventMarketplace.Infrastructure.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace EventMarketplace.Tests.Application.Commands.UnitTests.EventTests;

public class EditEventCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IBlobStorageService> _blob;
    private readonly Mock<IEventRepository> _eventRepo;
    //private readonly EditEventCommandHandler _handler;
    
    public EditEventCommandHandlerTests()
    {
        // _unitOfWork = new Mock<IUnitOfWork>();
        // _mapper = new Mock<IMapper>();
        // _blob = new Mock<IBlobStorageService>();
        // _eventRepo = new Mock<IEventRepository>();
        //
        // var configuration = new MapperConfiguration(cfg =>
        // {
        //     cfg.AddProfile<EventProfile>();
        // });
        //
        // var realMapper = configuration.CreateMapper();
        //
        // //_handler = new EditEventCommandHandler(_unitOfWork.Object, realMapper, _blob.Object);
        //
        // _unitOfWork.Setup(x => x.Events).Returns(_eventRepo.Object);
        // _unitOfWork.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
        //     .Returns(Task.CompletedTask);
        // _unitOfWork.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
        //     .Returns(Task.CompletedTask);

    }
    
    [Fact]
    public async Task Success_When_Data_Is_Valid()
    {
        // var eventToUpdate = new Event()
        // {
        //     ImageUrl = "events/old.jpg",
        //     DurationOfTheEvent = DurationOfTheEvent.Create(DateTime.UtcNow, DateTime.UtcNow.AddHours(1))
        // };
        //
        // _unitOfWork.Setup(x => x.Events.GetEventByIdAsync(It.IsAny<Guid>())).ReturnsAsync(eventToUpdate);
        //
        // var file = new FormFile(Stream.Null, 0, 0, "file", "file.jpg");
        // var dto = new EditEventDto()
        // {
        //     Title = "string",
        //     Description = "description",
        //     Price = 3.4,
        //     Image = file,
        //     AvailableTickets = 210,
        //     StartDateTime = DateTime.UtcNow.AddDays(1),
        //     EndDateTime = DateTime.UtcNow.AddDays(3),
        // };
        //
        // _blob.Setup(x => x.UploadOrReplaceFileAsync(file, "file", "events", CancellationToken.None))
        //     .ReturnsAsync("fake.jpg");
        // _unitOfWork.Setup(x => x.Events.UpdateEventAsync(eventToUpdate)).Returns(Task.CompletedTask);
        //
        // var command = new EditEventCommand(dto);
        //
        // await _handler.Handle(command, CancellationToken.None);
        //
        // _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        // _unitOfWork.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);

    }
}