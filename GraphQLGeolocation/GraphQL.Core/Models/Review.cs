using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GraphQL.Core.Classes;

namespace GraphQL.Core.Models
{
    public class Review : BaseEntity
    {
        public string Description { get; set; }
        
        public int PlaceId { get; set; }
        public int AccountId { get; set; }
        
        [ForeignKey("PlaceId")]
        public Place Place { get; set; }
        
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}