using MediatR;

namespace Auth.Core.Commands.LogoutUserCommand
{
    public class LogoutUserCommand : IRequest
    {
        public string Token { get; set; }
    }
}
