using MediatR;

namespace Auth.Core.Commands.LoginUserCommand
{
    public class LogoutUserCommand : IRequest
    {
        public string Token { get; set; }
    }
}
