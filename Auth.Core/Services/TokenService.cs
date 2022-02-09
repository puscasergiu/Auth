using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Core.Abstractions;
using Auth.Core.Constants;
using Auth.Core.Models;
using Auth.Core.Repositories;

namespace Auth.Core.Cryptography
{
    public class TokenService : ITokenService
    {
        private readonly IRevokedTokenRepository _revokedTokenRepository;
        private readonly ITokenSettings _tokenSettings;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly TokenCrypter _tokenCrypter;

        public TokenService(IRevokedTokenRepository revokedTokenRepository, ITokenSettings tokenSettings, IDateTimeProvider dateTimeProvider, TokenCrypter tokenCrypter)
        {
            _revokedTokenRepository = revokedTokenRepository ?? throw new ArgumentNullException(nameof(revokedTokenRepository));
            _tokenSettings = tokenSettings ?? throw new ArgumentNullException(nameof(tokenSettings));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _tokenCrypter = tokenCrypter ?? throw new ArgumentNullException(nameof(tokenCrypter));
        }

        public string CreateToken(Guid userId, string username)
        {
            var claims = new List<Claim>()
            {
                new Claim(Claims.UserId, userId.ToString()),
                new Claim(Claims.Username, username),
                new Claim(Claims.ExpirationDate, _dateTimeProvider.UtcNow.Add(_tokenSettings.LifeTime).Ticks.ToString())
            };

            return _tokenCrypter.EncodeToken(claims, Convert.FromBase64String(_tokenSettings.Salt));
        }

        public bool TryDecodeToken(string token, out TokenPayloadModel result)
        {
            result = null;
            if (!_tokenCrypter.TryDecodeToken(token, Convert.FromBase64String(_tokenSettings.Salt), out IList<Claim> claims))
            {
                return false;
            }

            var userIdClaim = claims.FirstOrDefault(c => c.Type == Claims.UserId);
            var usernameClaim = claims.FirstOrDefault(c => c.Type == Claims.Username);
            var expirationDateClaim = claims.FirstOrDefault(c => c.Type == Claims.ExpirationDate);
            if (userIdClaim == null || usernameClaim == null || expirationDateClaim == null)
            {
                return false;
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId) || !long.TryParse(expirationDateClaim.Value, out long expirationDateTicks) || expirationDateTicks < 0)
            {
                return false;
            }

            result = new TokenPayloadModel()
            {
                UserId = userId,
                Username = usernameClaim.Value,
                ExpirationDate = new DateTime(expirationDateTicks)
            };
            return true;
        }

        public async Task<bool> ValidateToken(string token, TokenPayloadModel tokenPayload)
        {
            if (tokenPayload.ExpirationDate < _dateTimeProvider.UtcNow || await _revokedTokenRepository.ExistsAsync(token))
            {
                return false;
            }

            return true;
        }
    }
}
