using System;

namespace Auth.Core.Cryptography
{
    public class TokenPayloadModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
