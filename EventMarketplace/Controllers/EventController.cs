using EventMarketplace.Application.Abstract.Dispatchers;
using EventMarketplace.Application.Commands.EventCommands;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response.EventResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMarketplace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllEvents([FromQuery] GetEventsQuery query)
        {
            return Ok(await queryDispatcher.QueryAsync<GetEventsQuery, EventListResponse>(query));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNewEvent([FromBody] CreateEventCommand command)
        {
            await commandDispatcher.SendAsync(command);
            return Created();
        }
    }
}
