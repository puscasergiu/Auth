using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Auth.Core.Cryptography
{
    public class HashCrypter
    {
        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            };

            return salt;
        }

        public string Hash(string text, byte[] salt)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(text));
            }

            if (salt is null)
            {
                throw new ArgumentNullException(nameof(salt));
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: text,
               salt: salt,
               prf: KeyDerivationPrf.HMACSHA256,
               iterationCount: 100000,
               numBytesRequested: 256 / 8));

            return hashed;
        }

        public bool Verify(string plainText, string hashedText, byte[] salt)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                throw new ArgumentException($"'{nameof(plainText)}' cannot be null or whitespace.", nameof(plainText));
            }

            if (string.IsNullOrWhiteSpace(hashedText))
            {
                throw new ArgumentException($"'{nameof(hashedText)}' cannot be null or whitespace.", nameof(hashedText));
            }

            if (salt is null)
            {
                throw new ArgumentNullException(nameof(salt));
            }

            return Hash(plainText, salt) == hashedText;
        }
    }
}
