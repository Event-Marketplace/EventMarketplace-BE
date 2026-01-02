
using EventMarketplace.Application.Commands.EventCommands;
using EventMarketplace.Application.Commands.EventCommands.DeleteEvent;
using EventMarketplace.Application.Commands.EventCommands.EditEvent;
using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.EventResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EventMarketplace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllEvents([FromQuery] GetEventsQuery query)
        {
            return Ok(await mediator.Send(query));
        }

        [Authorize]
        [HttpGet("organizer")]
        public async Task<IActionResult> GetOrganizerEvents([FromQuery] GetOrganizerEventsQuery query)
        {
            return Ok(await mediator.Send(query));
        }

        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminEvents([FromQuery] GetAdminEventsQuery query)
        {
            return Ok();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEvent([FromRoute] Guid id)
        {
            return Ok(await mediator.Send(new GetEventQuery(){EventId = id}));
        }
        
        [HttpGet("status-options")]
        public async Task<IActionResult> GetAllEventStatuses()
        {
            return Ok(mediator.Send(new GetEventStatusesQuery()));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNewEvent([FromForm] CreateEventDto Dto)
        {
            var command = new CreateEventCommand(Dto);
            await mediator.Send(command);
            return Created();
        }

        [HttpPut("submit-event/{eventId:guid}")]
        public async Task<IActionResult> SubmitEventToAdmin([FromRoute] Guid eventId)
        {
            var command = new SubmitEventCommand(eventId);
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> EditEvent([FromRoute] Guid id, [FromForm] EditEventDto Dto)
        {
            var command = new EditEventCommand(Dto);
            command.Dto.Id = id;
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] Guid id)
        {
            await mediator.Send(new DeleteEventCommand(id));
            return NoContent();
        }
        
    }
}
