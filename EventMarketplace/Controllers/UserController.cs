
using EventMarketplace.Application.Commands.UserCommands;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response;
using EventMarketplace.Application.Response.UserResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventMarketplace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        IMediator mediator
        ) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand command)
        {
            return Ok(await mediator.Send(command));
        }

        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfo([FromQuery] GetUserInfoQuery query)
        {
            return Ok(await mediator.Send(query));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUser()
        {
            await mediator.Send(new LogoutUserCommand());
            return NoContent();
        }
        
    }
}
