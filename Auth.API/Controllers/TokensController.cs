using System;
using System.Threading.Tasks;
using Auth.Core.Commands.VerifyTokenCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TokensController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("Verify")]
        [ProducesResponseType(200, Type = typeof(VerifyTokenCommandResponse))]
        public async Task<IActionResult> Verify(VerifyTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
