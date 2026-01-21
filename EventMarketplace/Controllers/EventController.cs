
using EventMarketplace.Application.Commands.EventCommands;
using EventMarketplace.Application.Commands.EventCommands.AdminFunctions;
using EventMarketplace.Application.Commands.EventCommands.EditEvent;
using EventMarketplace.Application.Dtos.EventDtos;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.EventResponse;
using EventMarketplace.Application.Services.Events.CreateEvent;
using EventMarketplace.Application.UseCases.Events.CreateEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EventMarketplace.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventController(IMediator mediator, IEventManagementService eventManagementService) : ControllerBase
    {
        #region GET

        [ResponseCache(Duration = 60)]
        [HttpGet]
        public async Task<IActionResult> GetAllEvents([FromQuery] GetEventsQuery query)
        {
            return Ok(await mediator.Send(query));
        }

        [Authorize(Roles = "Organizer")]
        [HttpGet("organizer")]
        public async Task<IActionResult> GetOrganizerEvents([FromQuery] GetOrganizerEventsQuery query)
        {
            return Ok(await mediator.Send(query));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminEvents([FromQuery] GetAdminEventsQuery query)
        {
            return Ok(await mediator.Send(query));
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


        #endregion

        #region POST

        [HttpPost]
        public async Task<IActionResult> AddNewEvent([FromForm] CreateEventRequest request, CancellationToken cancellationToken)
        {
            await eventManagementService.CreateEvent(request, cancellationToken);
            return Created();
        }

        #endregion
       
        #region PUT
        
        [HttpPut("submit-event/{eventId:guid}")]
        public async Task<IActionResult> SubmitEventToAdmin([FromRoute] Guid eventId)
        {
            var command = new SubmitEventCommand(eventId);
            await mediator.Send(command);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("approve-event/{eventId:guid}")]
        public async Task<IActionResult> ApproveEvent([FromRoute] Guid eventId)
        {
            var command = new ApproveEventCommand(eventId);
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPut("reject-event/{eventId:guid}")]
        public async Task<IActionResult> RejectEvent([FromRoute] Guid eventId, [FromBody] RejectEventCommand command)
        {
            await mediator.Send(command with { EventId = eventId });
            return NoContent();
        }
        
        #endregion

        #region PATCH

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> EditEvent([FromRoute] Guid id, [FromForm] EditEventRequest request, CancellationToken cancellationToken)
        {
            request.Id = id;
            await eventManagementService.EditEvent(request, cancellationToken);
            return NoContent();
        }

        #endregion

        #region DELETE

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await eventManagementService.DeleteEvent(id, cancellationToken);
            return NoContent();
        }

        #endregion
      
       
        
    }
}
