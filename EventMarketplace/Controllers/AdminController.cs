using EventMarketplace.Application.Services.Admin.Stats;
using EventMarketplace.Application.Services.Admin.Stats.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GetStatsQuery = EventMarketplace.Application.Services.Admin.Stats.Queries.GetStatsQuery;

namespace EventMarketplace.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IAdminStatsService adminStatsService) : ControllerBase
    {
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            return Ok(await adminStatsService.GetStats(new GetStatsQuery()));
        }

        [HttpGet("alerts")]
        public async Task<IActionResult> GetAlerts()
        {
            return Ok(await adminStatsService.GetAlerts(new GetAlertQuery()));
        }
        
    }
}
