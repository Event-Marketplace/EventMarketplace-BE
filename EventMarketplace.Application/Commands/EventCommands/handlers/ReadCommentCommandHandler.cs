using EventMarketplace.Application.Patterns;
using EventMarketplace.Application.Utils;
using MediatR;

namespace EventMarketplace.Application.Commands.EventCommands;

public class ReadCommentCommandHandler(
    IUnitOfWork unitOfWork, 
    IUserService userService) : IRequestHandler<ReadCommentCommand>
{
    public async Task Handle(ReadCommentCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = userService.GetUserIdFromContext();
        
        var eventComments = await unitOfWork.EventComments
            .GetEventCommentListByEventIdAsync(request.EventId, currentUserId);
        
        eventComments.ForEach(x => x.SetReadComment());
        await unitOfWork.EventComments.UpdateEventCommentListAsync(eventComments); 
    }
}