using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Domain.Enums;
using MediatR;

namespace EventMarketplace.Application.Queries;

public sealed class GetOrganizerEventsQuery : IRequest<EventListResponse>
{
    public int PageNumber { get; set; }
    public string? Title { get; set; }
    public EventStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}