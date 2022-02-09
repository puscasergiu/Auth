using MediatR;

namespace Auth.Core.Commands.LoginUserCommand
{
    public class LoginUserCommand : IRequest<LoginUserCommandResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
