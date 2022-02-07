using MediatR;

namespace Auth.Core.Commands.RegisterUserCommand
{
    public class RegisterUserCommand : IRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
