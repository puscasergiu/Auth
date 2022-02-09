using System;
using Auth.Core.Models;

namespace Auth.API.Models.Options
{
    public class TokenOptions : ITokenSettings
    {
        public const string SectionName = "Token";

        public string Salt { get; set; }
        public TimeSpan LifeTime { get; set; }
    }
}
