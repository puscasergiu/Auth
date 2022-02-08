using System;

namespace Auth.Core.Exceptions
{
    public class TokenDecodeException : DomainException
    {
        public TokenDecodeException()
        {
        }

        public TokenDecodeException(string message) : base(message)
        {
        }

        public TokenDecodeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
