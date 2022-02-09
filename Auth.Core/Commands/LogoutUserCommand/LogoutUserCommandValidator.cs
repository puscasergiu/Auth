using FluentValidation;

namespace Auth.Core.Commands.LogoutUserCommand
{
    public class LogoutUserCommandValidator : AbstractValidator<LogoutUserCommand>
    {
        public LogoutUserCommandValidator()
        {
            RuleFor(u => u.Token)
                .NotEmpty();
        }
    }
}
