using System;
using System.Collections.Generic;

namespace GraphQL.Core.Models
{
    public class Place
    {
        public Place()
        {
            this.Tags = new List<Guid>();
        }
        
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public Location Location { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public DateTimeOffset Modified { get; set; }
        
        public List<Guid> Tags { get; set; }
    }
}