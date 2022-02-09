using System;
using Auth.Core.Abstractions;

namespace Auth.API.Miscellaneous
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
