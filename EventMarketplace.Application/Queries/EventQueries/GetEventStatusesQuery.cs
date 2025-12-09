using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Domain.Enums;
using MediatR;

namespace EventMarketplace.Application.Queries;

public class GetEventStatusesQuery : IRequest<List<EventStatusResponse>>
{
    
}