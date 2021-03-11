using System;
using System.ComponentModel.DataAnnotations;

namespace GraphQL.Core.Models
{
    public class Location
    {
        [Key]
        public Guid Id { get; set; }
        
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }
    }
}