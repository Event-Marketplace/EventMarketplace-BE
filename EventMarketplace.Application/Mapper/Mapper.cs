using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Domain.Entities;
using EventMarketplace.Domain.Enums;

namespace EventMarketplace.Infrastructure.Mapper;

public static class Mapper
{
    public static EventResponse MapToEventResponse(this Event @event)
    {
        return new EventResponse()
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            AvailableTickets = @event.AvailableTickets,
            Price = @event.Price,
            ImageUrl = @event.ImageUrl,
            StartDate = @event.DurationOfTheEvent.StartEvent,
            EndDate = @event.DurationOfTheEvent.EndEvent,
            CreatedAt = @event.CreatedAt,
            UpdatedAt = @event.UpdatedAt,
            Status = @event.EventStatus,
            StatusDisplayName = @event.EventStatus.GetDisplayName(),
        };
    }
}