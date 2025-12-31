using EventMarketplace.Application.Queries.AdminQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMarketplace.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IMediator mediator) : ControllerBase
    {
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            return Ok(await mediator.Send(new GetStatsQuery()));
        }

        [HttpGet("alerts")]
        public async Task<IActionResult> GetAlerts()
        {
            return Ok(await mediator.Send(new GetAlertsQuery()));
        }
        
    }
}
