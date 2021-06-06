using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraphQL.Core.Classes;

namespace GraphQL.Core.Models
{
    public class Place : BaseEntity
    {
        public Place()
        {
            Review = new List<Review>();
        }

        public string Name { get; set; }
        public int LocationId { get; set; }
        public int CityId { get; set; }
        
        [ForeignKey("CityId")]
        public City City { get; set; }
        
        [ForeignKey("LocationId")]
        public Location Location { get; set; }
        
        public ICollection<Review> Review { get; set; }
    }
}