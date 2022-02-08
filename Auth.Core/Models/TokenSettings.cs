using System;

namespace Auth.Core.Models
{
    public class TokenSettings
    {
        public string Salt { get; set; }
        public TimeSpan LifeTime { get; set; }
    }
}
