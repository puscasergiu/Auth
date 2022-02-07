using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Auth.Core.Cryptography
{
    public class HashCrypter
    {
        public string Hash(string text, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: text,
               salt: salt,
               prf: KeyDerivationPrf.HMACSHA256,
               iterationCount: 100000,
               numBytesRequested: 256 / 8));

            return hashed;
        }

        public (string hashedText, string salt) Hash(string text)
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            };

            var hashed = Hash(text, salt);

            return (hashed, Convert.ToBase64String(salt));
        }

        public string Hash(string text, string salt)
        {
            string hashed = Hash(text, Convert.FromBase64String(salt));

            return hashed;
        }

        public bool Verify(string plainText, string hashedText, string salt)
        {
            return Hash(plainText, Convert.FromBase64String(salt)) == hashedText;
        }
    }
}
