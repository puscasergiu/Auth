using FluentValidation;

namespace Auth.Core.Commands.LoginUserCommand
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty();

            RuleFor(u => u.Password)
                .NotEmpty();
        }
    }
}
