using System;
using System.Collections.Generic;

namespace GraphQL.Core.Models
{
    public class Account
    {
        public Account()
        {
            this.Friends = new List<Guid>();
            this.AppearsIn = new List<Place>();
            this.SharedTags = new List<Tag>();
        }
        
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public List<Guid> Friends { get; set; }
        
        public List<Place> AppearsIn { get; set; }
        
        public List<Tag> SharedTags { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public DateTimeOffset Modified { get; set; }
    }
}