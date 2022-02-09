using System;
using System.Threading.Tasks;
using Auth.Core.Models;

namespace Auth.Core.Cryptography
{
    public interface ITokenService
    {
        string CreateToken(Guid userId, string username);
        bool TryDecodeToken(string token, out TokenPayloadModel result);
        Task<bool> ValidateToken(string token, TokenPayloadModel tokenPayload);
    }
}