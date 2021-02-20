using System;

namespace GraphQL.Core.Models
{
    public class Droid : Character
    {
        public DateTimeOffset Manufactured { get; set; }
        
        public TimeSpan ChargePeriod { get; set; }
        
        public string PrimaryFunction { get; set; }
    }
}