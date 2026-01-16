
using EventMarketplace.Application.Commands.UserCommands;
using EventMarketplace.Application.Queries;
using EventMarketplace.Application.Response;
using EventMarketplace.Application.Response.UserResponse;
using EventMarketplace.Application.Services.Users.Get;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EventMarketplace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        IMediator mediator
        ) : ControllerBase
    {

        [HttpGet("username-from-service/{userId:int}")]
        public async Task<ActionResult<string>> GetUser([FromRoute] int userId, [FromServices] IGetUserUseCase getUserUseCase)
        {
            return Ok(getUserUseCase.GetUserById(userId));
        }
        
        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfo([FromQuery] GetUserInfoQuery query)
        {
            return Ok(await mediator.Send(query));
        }
        
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

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutUser()
        {
            await mediator.Send(new LogoutUserCommand());
            return NoContent();
        }

        [EnableRateLimiting("RegenerateTokensPolicy")]
        [HttpPost("auth-refresh")]
        public async Task<IActionResult> RegenerateTokens()
        {
            return Ok(await mediator.Send(new RegenerateTokensCommand()));
        }
        
    }
}
