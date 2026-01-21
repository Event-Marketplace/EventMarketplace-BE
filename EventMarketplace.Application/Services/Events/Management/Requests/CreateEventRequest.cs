using EventMarketplace.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace EventMarketplace.Application.Services.Events.CreateEvent;

public class CreateEventRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public decimal Price { get; set; }
    public int AvailableTicketsCount { get; set; }
    public IFormFile Image{ get; set; }
    public LocationType LocationType { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? EventPlaceDescription { get; set; }
}