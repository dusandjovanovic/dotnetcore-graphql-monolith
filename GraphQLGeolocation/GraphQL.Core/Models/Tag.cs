using System;
using System.ComponentModel.DataAnnotations;

namespace GraphQL.Core.Models
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }
        
        public string Description { get; set; }
        
        public Location Location { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public DateTimeOffset Modified { get; set; }
    }
}