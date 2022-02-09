using System;

namespace Auth.Core.Commands.VerifyTokenCommand
{
    public class VerifyTokenCommandResponse
    {
        private VerifyTokenCommandResponse()
        {
        }

        public static VerifyTokenCommandResponse Invalid(string reason)
        {
            return new VerifyTokenCommandResponse()
            {
                IsValid = false,
                NotValidReason = reason
            };
        }

        public static VerifyTokenCommandResponse Valid(DateTime expirationDate)
        {
            return new VerifyTokenCommandResponse()
            {
                IsValid = true,
                ExpirationDate = expirationDate
            };
        }

        public bool IsValid { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string NotValidReason { get; set; }
    }
}
