
using EventMarketplace.Application.Commands.EventCommands;
using EventMarketplace.Application.Commands.EventCommands.DeleteEvent;
using EventMarketplace.Application.Commands.EventCommands.EditEvent;
using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.EventResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMarketplace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController(IMediator mediator) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllEvents([FromQuery] GetEventsQuery query)
        {
            return Ok(await mediator.Send(query));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEvent([FromRoute] Guid id)
        {
            return Ok(await mediator.Send(new GetEventQuery(){EventId = id}));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNewEvent([FromForm] CreateEventDto Dto)
        {
            var command = new CreateEventCommand(Dto);
            await mediator.Send(command);
            return Created();
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> EditEvent([FromRoute] Guid id, [FromForm] EditEventCommand command)
        {
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
