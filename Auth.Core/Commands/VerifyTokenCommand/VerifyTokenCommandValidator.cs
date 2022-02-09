using FluentValidation;

namespace Auth.Core.Commands.VerifyTokenCommand
{
    public class VerifyTokenCommandValidator : AbstractValidator<VerifyTokenCommand>
    {
        public VerifyTokenCommandValidator()
        {
            RuleFor(u => u.Token)
                .NotEmpty();
        }
    }
}
