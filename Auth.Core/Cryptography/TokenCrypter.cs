using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Auth.Core.Cryptography
{
    public class TokenCrypter
    {
        private readonly HashCrypter _hashCrypter;

        public TokenCrypter(HashCrypter hashCrypter)
        {
            _hashCrypter = hashCrypter ?? throw new ArgumentNullException(nameof(hashCrypter));
        }

        public string EncodeToken(IEnumerable<Claim> claims, byte[] salt)
        {
            if (claims is null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            if (salt is null)
            {
                throw new ArgumentNullException(nameof(salt));
            }

            var publicText = string.Join(";", claims.Select(s => $"{s.Type}:{s.Value}"));
            var publicTextBase64Encoded = Base64Encode(publicText);
            var hash = _hashCrypter.Hash(publicTextBase64Encoded, salt);

            return string.Join(".", publicTextBase64Encoded, hash);
        }

        public bool TryDecodeToken(string token, byte[] salt, out IList<Claim> result)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException($"'{nameof(token)}' cannot be null or whitespace.", nameof(token));
            }

            if (salt is null)
            {
                throw new ArgumentNullException(nameof(salt));
            }

            result = null;
            if (!TrySplitToken(token, out string claimsInBase64, out string hash))
            {
                return false;
            }

            if (!_hashCrypter.Verify(claimsInBase64, hash, salt))
            {
                return false;
            }

            if (!TryBase64Decode(claimsInBase64, out string claims))
            {
                return false;
            }

            var splittedClaims = claims.Split(';');
            result = new List<Claim>();
            foreach (var claim in splittedClaims)
            {
                var splittedClaim = claim.Split(':');
                if (splittedClaim.Length != 2)
                {
                    result = null;
                    return false;
                }
                result.Add(new Claim(splittedClaim[0], splittedClaim[1]));
            }

            return true;
        }

        private static bool TrySplitToken(string token, out string claims, out string hash)
        {
            var tokenParts = token.Split('.');
            if (tokenParts.Length != 2)
            {
                claims = null;
                hash = null;
                return false;
            }

            claims = tokenParts[0];
            hash = tokenParts[1];
            return true;
        }

        private static string Base64Encode(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                throw new ArgumentException($"'{nameof(plainText)}' cannot be null or whitespace.", nameof(plainText));
            }

            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private static bool TryBase64Decode(string base64EncodedText, out string result)
        {
            if (string.IsNullOrWhiteSpace(base64EncodedText))
            {
                throw new ArgumentException($"'{nameof(base64EncodedText)}' cannot be null or whitespace.", nameof(base64EncodedText));
            }

            Span<byte> buffer = new(new byte[base64EncodedText.Length]);
            var convertResult = Convert.TryFromBase64String(base64EncodedText, buffer, out int bytesParsed);

            if (!convertResult)
            {
                result = null;
                return false;
            }

            result = Encoding.UTF8.GetString(buffer);
            return true;
        }
    }
}
