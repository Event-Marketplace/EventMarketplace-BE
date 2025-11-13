using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Response.EventResponse;

namespace EventMarketplace.Application.Queries;

public class GetEventQuery : IQuery<EventResponse>
{
    public Guid EventId { get; set; }
}