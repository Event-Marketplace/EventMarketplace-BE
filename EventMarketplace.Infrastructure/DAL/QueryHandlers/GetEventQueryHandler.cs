using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Exceptions;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Infrastructure.DAL.QueryHandlers;

public class GetEventQueryHandler(EventMarketplaceDbContext context) : IQueryHandler<GetEventQuery, EventResponse>
{
    public async Task<EventResponse> ExecuteHandleAsync(GetEventQuery query, CancellationToken cancellationToken)
    {
        var @event = await context.Events.SingleOrDefaultAsync(x => x.Id == query.EventId, cancellationToken) 
            ?? throw new AppException("Brak wydarzenia w bazie danych.");

        return @event.MapToEventResponse();
    }
}