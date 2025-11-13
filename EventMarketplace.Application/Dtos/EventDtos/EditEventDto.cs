using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace EventMarketplace.Application.Dtos.EventDtos;

public class EditEventDto
{
    [SwaggerIgnore]
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public double? Price { get; set; }
    public int? AvailableTickets { get; set; }
    public IFormFile? Image{ get; set; }
}