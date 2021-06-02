using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraphQL.Core.Models
{
    public class Place
    {
        public Place()
        {
            Tags = new List<Tag>();
        }
        
        [Key]
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public Guid LocationId { get; set; }
        
        public Location Location { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public DateTimeOffset Modified { get; set; }
        
        public ICollection<Tag> Tags { get; set; }
    }
}