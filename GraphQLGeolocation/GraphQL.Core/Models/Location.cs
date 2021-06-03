using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraphQL.Core.Classes;

namespace GraphQL.Core.Models
{
    public class Location : BaseEntity
    {
        public float Latitude { get; set; }
        
        public float Longitude { get; set; }
        
        public int PlaceId { get; set; }
        
        [ForeignKey("PlaceId")]
        public Place Place { get; set; }
    }
}