using Auth.Core.Validators;
using FluentValidation;

namespace Auth.Core.Commands.RegisterUserCommand
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(u => u.FirstName)
              .NotEmpty()
              .MinimumLength(4)
              .MaximumLength(50);

            RuleFor(u => u.LastName)
              .NotEmpty()
              .MinimumLength(4)
              .MaximumLength(50);

            RuleFor(u => u.Username)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(50);

            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(50)
                .Password();
        }
    }
}
