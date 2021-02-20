using System;

namespace GraphQL.Data.Services
{
    public class ClockService : IClockService
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}