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
            _hashCrypter = hashCrypter;
        }

        public string EncodeToken(IEnumerable<Claim> claims, string salt)
        {
            var publicText = string.Join(";", claims.Select(s => $"{s.Type}:{s.Value}"));
            var publicTextBase64 = Base64Encode(publicText);
            var hash = _hashCrypter.Hash(publicTextBase64, salt);

            return string.Join(".", publicTextBase64, hash);
        }

        public IEnumerable<Claim> DecodeToken(string token, string salt)
        {
            string[] parts = token.Split('.');
            if (parts.Length != 2)
            {
                throw new Exception("Token is in wrong format");
            }

            string claimsBase64Encoded = parts[0];
            string hash = parts[1];
            if (!_hashCrypter.Verify(claimsBase64Encoded, hash, salt))
            {
                throw new Exception("Hash could not be verified");
            }

            var claimsString = Base64Decode(claimsBase64Encoded);
            return claimsString.Split(';').Select(claim =>
            {
                var splittedClaim = claim.Split(':');
                return new Claim(splittedClaim[0], splittedClaim[1]);
            });
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
