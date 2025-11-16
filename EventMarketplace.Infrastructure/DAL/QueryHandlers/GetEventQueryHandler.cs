
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Infrastructure.Mapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public class GetEventQueryHandler(EventMarketplaceDbContext context) : IRequestHandler<GetEventQuery, EventResponse>
{
    public async Task<EventResponse> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        var @event = await context.Events.SingleOrDefaultAsync(x => x.Id == request.EventId, cancellationToken) 
                     ?? throw new AppException("Brak wydarzenia w bazie danych.");

        return @event.MapToEventResponse();
    }
}