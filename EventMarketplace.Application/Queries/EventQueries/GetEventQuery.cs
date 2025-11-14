
using EventMarketplace.Application.Response.EventResponse;
using MediatR;

namespace EventMarketplace.Application.Queries;

public class GetEventQuery : IRequest<EventResponse>
{
    public Guid EventId { get; set; }
}