using System.Text.RegularExpressions;
using FluentValidation;

namespace Auth.Core.Validators
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var lowercase = new Regex("[a-z]+");
            var uppercase = new Regex("[A-Z]+");
            var digit = new Regex("(\\d)+");
            var symbol = new Regex("(\\W)+");

            return ruleBuilder
                .Must(pw => lowercase.IsMatch(pw) && uppercase.IsMatch(pw) && (digit.IsMatch(pw) || symbol.IsMatch(pw)))
                .WithMessage("Password must contain 1 lower case, 1 upper case and one digit or symbol");
        }

        public static IRuleBuilderOptions<T, string> Token<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var tokenRegex = new Regex("([A-Za-z]{1}[.]{1}[A-Za-z]{1})");
           
            return ruleBuilder
                .NotEmpty()
                .Must(token => tokenRegex.IsMatch(token))
                .WithMessage("Token must have public details and hash separated by a dot");
        }
    }
}
