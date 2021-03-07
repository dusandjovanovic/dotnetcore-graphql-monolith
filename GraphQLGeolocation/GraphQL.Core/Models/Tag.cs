using System;

namespace GraphQL.Core.Models
{
    public class Tag
    {
        public Guid Id { get; set; }
        
        public string Description { get; set; }
        
        public Location Location { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public DateTimeOffset Modified { get; set; }
    }
}