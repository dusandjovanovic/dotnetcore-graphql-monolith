using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GraphQL.Core.Models
{
    public class Account
    {
        public Account()
        {
            Friends = new List<Account>();
            AppearsIn = new List<Place>();
            SharedTags = new List<Tag>();
        }
        
        [Key]
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public ICollection<Account> Friends { get; set; }
        
        public ICollection<Place> AppearsIn { get; set; }
        
        public ICollection<Tag> SharedTags { get; set; }

        public DateTimeOffset Created { get; set; }
        
        public DateTimeOffset Modified { get; set; }
    }
}