using System;

namespace GraphQL.Data.Services
{
    public interface IClockService
    {
        DateTimeOffset UtcNow { get; }
    }
}