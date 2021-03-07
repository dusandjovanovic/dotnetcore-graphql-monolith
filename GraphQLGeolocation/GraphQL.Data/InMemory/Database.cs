using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Core.Models;

namespace GraphQL.Data.InMemory
{
    public class Database
    {
        static Database()
        {
            var created = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
            
            
            var tag1 = new Tag()
            {
                Id = new Guid("3ee34c3b-c1a0-4b7b-9375-c5a221d49e68"),
                Description = "Hello world",
                Created = created,
                Modified = created,
                Location =
                {
                    Latitude = 63.530,
                    Longitude = 10.395
                }
            };

            var place1 = new Place()
            {
                Id = new Guid("112bd693-2027-4804-bf40-ed427fe76fda"),
                Name = "London",
                Location =
                {
                    Latitude = 51.507,
                    Longitude = 0.112
                }
            };
            
            var place2 = new Place()
            {
                Id = new Guid("122bd693-2027-4804-bf40-ed427fe76fda"),
                Name = "Trondheim",
                Location =
                {
                    Latitude = 63.430,
                    Longitude = 10.395
                }
            };

            var account1 = new Account()
            {
                Id = new Guid("1ae34c3b-c1a0-4b7b-9375-c5a221d49e68"),
                Email = "someone@example.com",
                Name = "R2-D2",
                Created = created,
                Modified = created,
            };
            account1.Friends.Add(new Guid("2ae34c3b-c1a0-4b7b-9375-c5a221d49e68"));
            account1.SharedTags.Add(tag1);
            account1.AppearsIn.Add(place1);
            account1.AppearsIn.Add(place2);

            var account2 = new Account()
            {
                Id = new Guid("2ae34c3b-c1a0-4b7b-9375-c5a221d49e68"),
                Email = "someone@example.com",
                Name = "C3PO",
                Created = created,
                Modified = created,
            };
            account1.Friends.Add(new Guid("1ae34c3b-c1a0-4b7b-9375-c5a221d49e68"));
            account2.AppearsIn.Add(place2);

            Tags = new List<Tag>()
            {
                tag1
            };

            Places = new List<Place>()
            {
                place1,
                place2
            };
            
            Accounts = new List<Account>()
            {
                account1,
                account2,
            };
        }

        public static List<Account> Accounts { get; }
        
        public static List<Place> Places { get; }
        
        public static List<Tag> Tags { get; }
    }
}