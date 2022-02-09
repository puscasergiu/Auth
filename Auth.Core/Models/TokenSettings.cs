using System;

namespace Auth.Core.Models
{
    public interface ITokenSettings
    {
        public string Salt { get; set; }
        public TimeSpan LifeTime { get; set; }
    }
}
