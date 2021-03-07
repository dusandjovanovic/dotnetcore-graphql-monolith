using System;
using System.Collections.Generic;

namespace GraphQL.Core.Models
{
    public class Account
    {
        public Account()
        {
            this.Friends = new List<Guid>();
            this.AppearsIn = new List<Guid>();
            this.SharedTags = new List<Guid>();
        }
        
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public List<Guid> Friends { get; set; }
        
        public List<Guid> AppearsIn { get; set; }
        
        public List<Guid> SharedTags { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public DateTimeOffset Modified { get; set; }
    }
}