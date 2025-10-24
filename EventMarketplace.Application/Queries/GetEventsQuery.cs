using EventMarketplace.Application.Abstract;
using EventMarketplace.Application.Response.EventResponse;

namespace EventMarketplace.Application.Queries;

public sealed class GetEventsQuery : IQuery<EventListResponse>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Title { get; set; }
    public double? StartPrice { get; set; }
    public double? EndPrice { get; set; }
    public int PageNumber { get; set; }
}