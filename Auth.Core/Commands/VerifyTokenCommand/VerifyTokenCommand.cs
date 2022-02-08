using MediatR;

namespace Auth.Core.Commands.VerifyTokenCommand
{
    public class VerifyTokenCommand : IRequest<VerifyTokenCommandResponse>
    {
        public string Token { get; set; }
    }
}
