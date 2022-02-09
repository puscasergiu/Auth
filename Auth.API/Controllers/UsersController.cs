using System;
using System.Threading.Tasks;
using Auth.API.Models;
using Auth.Core.Commands.LoginUserCommand;
using Auth.Core.Commands.LogoutUserCommand;
using Auth.Core.Commands.RegisterUserCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("Register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400, Type = typeof(DomainErrorModel))]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("Login")]
        [ProducesResponseType(200, Type = typeof(LoginUserCommandResponse))]
        [ProducesResponseType(400, Type = typeof(DomainErrorModel))]
        public async Task<LoginUserCommandResponse> Login(LoginUserCommand command)
        {
            var loginResult = await _mediator.Send(command);
            return loginResult;
        }

        [HttpPost("Logout")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Logout(LogoutUserCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
