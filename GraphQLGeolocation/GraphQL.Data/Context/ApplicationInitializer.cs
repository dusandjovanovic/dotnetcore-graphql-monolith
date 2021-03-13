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
                Id = Guid.NewGuid(),
                Description = "Hello world",
                Created = created,
                Modified = created,
                Location = new Location
                {
                    Id = Guid.NewGuid(),
                    Latitude = 63.530,
                    Longitude = 10.395
                }
            };

            var place1 = new Place()
            {
                Id = Guid.NewGuid(),
                Name = "London",
                Location = new Location
                {
                    Id = Guid.NewGuid(),
                    Latitude = 51.507,
                    Longitude = 0.112
                }
            };

            var place2 = new Place()
            {
                Id = Guid.NewGuid(),
                Name = "Trondheim",
                Location = new Location
                {
                    Id = Guid.NewGuid(),
                    Latitude = 63.430,
                    Longitude = 10.395
                }
            };

            var account1 = new Account()
            {
                Id = Guid.NewGuid(),
                Email = "someone@example.com",
                DateOfBirth = DateTime.Now,
                Name = "R2-D2",
                Created = created,
                Modified = created,
            };

            var account2 = new Account()
            {
                Id = Guid.NewGuid(),
                Email = "someone@example.com",
                DateOfBirth = DateTime.Now,
                Name = "C3PO",
                Created = created,
                Modified = created,
            };
            
            context.Accounts.Add(account1);
            context.Accounts.Add(account2);
            context.Places.Add(place1);
            context.Places.Add(place2);
            context.Tags.Add(tag1);
            context.SaveChanges();

            account1.Friends.Add(account2);
            account1.SharedTags.Add(tag1);
            account1.AppearsIn.Add(place1);
            account1.AppearsIn.Add(place2);
            context.SaveChanges();

            account2.Friends.Add(account1);
            account2.AppearsIn.Add(place2);
            context.SaveChanges();
        }
    }
}