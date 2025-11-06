namespace EventMarketplace.Application.Dtos.EventDtos;

public class CreateEventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public double Price { get; set; }
    public int AvailableTicketsCount { get; set; }
    public string ImageUrl { get; set; }
}