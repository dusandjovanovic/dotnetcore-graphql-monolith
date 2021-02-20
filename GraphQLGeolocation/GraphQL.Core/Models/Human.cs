using System;

namespace GraphQL.Core.Models
{
    public class Human : Character
    {
        public DateTime DateOfBirth { get; set; }
        
        public string HomePlanet { get; set; }
    }
}