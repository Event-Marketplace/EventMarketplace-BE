
using EventMarketplace.Application.Response.EventResponse;
using MediatR;

namespace EventMarketplace.Application.Queries;

public sealed class GetEventsQuery : IRequest<EventListResponse>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Title { get; set; }
    public decimal? StartPrice { get; set; }
    public decimal? EndPrice { get; set; }
    public int PageNumber { get; set; }
}