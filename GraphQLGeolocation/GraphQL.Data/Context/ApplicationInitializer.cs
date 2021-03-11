using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Core.Models;

namespace GraphQL.Data.Context
{
    public class ApplicationInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            context.Database.EnsureCreated();

            if (context.Accounts.Any()) return;

            var created = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var tag1 = new Tag()
            {
                Id = new Guid("3ee34c3b-c1a0-4b7b-9375-c5a221d49e68"),
                Description = "Hello world",
                Created = created,
                Modified = created,
                Location = new Location
                {
                    Id = new Guid("-1"),
                    Latitude = 63.530,
                    Longitude = 10.395
                }
            };

            var place1 = new Place()
            {
                Id = new Guid("112bd693-2027-4804-bf40-ed427fe76fda"),
                Name = "London",
                Location = new Location
                {
                    Id = new Guid("0"),
                    Latitude = 51.507,
                    Longitude = 0.112
                }
            };

            var place2 = new Place()
            {
                Id = new Guid("122bd693-2027-4804-bf40-ed427fe76fda"),
                Name = "Trondheim",
                Location = new Location
                {
                    Id = new Guid("1"),
                    Latitude = 63.430,
                    Longitude = 10.395
                }
            };

            var account1 = new Account()
            {
                Id = new Guid("1ae34c3b-c1a0-4b7b-9375-c5a221d49e68"),
                Email = "someone@example.com",
                DateOfBirth = DateTime.Now,
                Name = "R2-D2",
                Created = created,
                Modified = created,
            };

            var account2 = new Account()
            {
                Id = new Guid("2ae34c3b-c1a0-4b7b-9375-c5a221d49e68"),
                Email = "someone@example.com",
                DateOfBirth = DateTime.Now,
                Name = "C3PO",
                Created = created,
                Modified = created,
            };

            account1.Friends.Add(account2);
            account1.SharedTags.Add(tag1);
            account1.AppearsIn.Add(place1);
            account1.AppearsIn.Add(place2);

            account2.Friends.Add(account1);
            account2.AppearsIn.Add(place2);
            
            context.Accounts.Add(account1);
            context.Accounts.Add(account2);
            context.Places.Add(place1);
            context.Places.Add(place2);
            context.Tags.Add(tag1);
            context.SaveChanges();
        }
    }
}