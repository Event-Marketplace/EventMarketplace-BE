using EventMarketplace.Application.Abstract.Dispatchers;
using EventMarketplace.Application.Commands.UserCommands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMarketplace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        ICommandDispatcher commandDispatcher
        ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
        {
            await commandDispatcher.SendAsync(command);
            return NoContent();
        }
    }
}
