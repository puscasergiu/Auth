using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Core.Constants;
using Auth.Core.Exceptions;
using Auth.Core.Models;
using Auth.Core.Repositories;

namespace Auth.Core.Cryptography
{
    public class TokenService : ITokenService
    {
        private readonly IRevokedTokenRepository _revokedTokenRepository;
        private readonly TokenCrypter _tokenCrypter;
        private readonly TokenSettings _tokenSettings;

        public TokenService(IRevokedTokenRepository revokedTokenRepository, TokenCrypter tokenCrypter, TokenSettings tokenSettings)
        {
            _revokedTokenRepository = revokedTokenRepository ?? throw new ArgumentNullException(nameof(revokedTokenRepository));
            _tokenCrypter = tokenCrypter ?? throw new ArgumentNullException(nameof(tokenCrypter));
            _tokenSettings = tokenSettings ?? throw new ArgumentNullException(nameof(tokenSettings));
        }

        public string CreateToken(Guid userId, string username)
        {
            var claims = new List<Claim>()
            {
                new Claim(Claims.UserId, userId.ToString()),
                new Claim(Claims.Username, username),
                new Claim(Claims.ExpirationDate, DateTime.UtcNow.Add(_tokenSettings.LifeTime).Ticks.ToString())
            };

            return _tokenCrypter.EncodeToken(claims, _tokenSettings.Salt);
        }

        /// <inheritdoc/>
        public TokenPayloadModel DecodeToken(string token)
        {
            try
            {
                var claims = _tokenCrypter.DecodeToken(token, _tokenSettings.Salt);

                TokenPayloadModel payload = new()
                {
                    UserId = Guid.Parse(claims.First(c => c.Type == Claims.UserId).Value),
                    Username = claims.First(c => c.Type == Claims.Username).Value,
                    ExpirationDate = new DateTime(long.Parse(claims.First(c => c.Type == Claims.ExpirationDate).Value))
                };

                return payload;
            }
            catch (Exception e)
            {
                throw new TokenDecodeException($"Failed to decode the token", e);
            }
        }

        public async Task<bool> ValidateToken(string token, TokenPayloadModel tokenPayload)
        {
            if (tokenPayload.ExpirationDate < DateTime.UtcNow || await _revokedTokenRepository.ExistsAsync(token))
            {
                return false;
            }

            return true;
        }
    }
}
