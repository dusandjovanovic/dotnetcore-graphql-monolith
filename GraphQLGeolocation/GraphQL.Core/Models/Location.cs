using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraphQL.Core.Classes;

namespace GraphQL.Core.Models
{
    public class Location : BaseEntity
    {
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }
    }
}