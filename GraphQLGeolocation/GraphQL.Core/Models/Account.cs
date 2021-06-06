using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GraphQL.Core.Classes;

namespace GraphQL.Core.Models
{
    public class Account : BaseEntity
    {
        public Account()
        {
            Friends = new List<Account>();
            Reviews = new List<Review>();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Account> Friends { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}