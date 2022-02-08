using System;
using System.Threading.Tasks;
using Auth.Core.Exceptions;

namespace Auth.Core.Cryptography
{
    public interface ITokenService
    {
        string CreateToken(Guid userId, string username);
        /// <exception cref="TokenDecodeException"></exception>
        TokenPayloadModel DecodeToken(string token);
        Task<bool> ValidateToken(string token, TokenPayloadModel tokenPayload);
    }
}