using EventMarketplace.Application.Abstract.Dispatchers;
using EventMarketplace.Application.Commands.UserCommands;
using EventMarketplace.Application.Response;
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
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
        {
            await commandDispatcher.SendAsync(command);
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand command)
        {
            return Ok(await commandDispatcher.SendAsync<LoginUserCommand, LoginUserResponse>(command));
        }
        
    }
}
