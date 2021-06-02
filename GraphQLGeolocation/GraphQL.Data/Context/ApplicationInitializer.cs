using System;
using System.Linq;
using GraphQL.Core.Models;

namespace GraphQL.Data.Context
{
    public class ApplicationInitializer
    {
        public static void Initialize(ApplicationContext context)
        {
            if (context.Accounts.Any()) return;

            var created = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var location1 = new Location
            {
                Id = Guid.Parse("42944bb8-a3c1-480d-826c-8d2c5767ec61"),
                Latitude = 63.530,
                Longitude = 10.395
            };

            var location2 = new Location
            {
                Id = Guid.Parse("0b4348b7-2578-4c42-b470-0ed02e8b0516"),
                Latitude = 51.507,
                Longitude = 0.112
            };

            var location3 = new Location
            {
                Id = Guid.Parse("bd7aba23-2b84-457d-a560-ad95e18b51e6"),
                Latitude = 63.430,
                Longitude = 10.395
            };

            var tag1 = new Tag()
            {
                Id = Guid.Parse("7bdb8faf-4c71-4c56-906b-c1d2ac593a90"),
                Description = "Hello world",
                Created = created,
                Modified = created,
                LocationId = location1.Id
            };

            var place1 = new Place()
            {
                Id = Guid.Parse("bb63f8f2-8c5d-4e64-b963-6032332ad3d9"),
                Name = "London",
                LocationId = location2.Id
            };

            var place2 = new Place()
            {
                Id = Guid.Parse("7ad3c83a-424d-44dd-ad70-0b545132997a"),
                Name = "Trondheim",
                LocationId = location3.Id
            };

            var account1 = new Account()
            {
                Id = Guid.Parse("fc3190ee-82cf-4d85-ab3d-0e7001619ade"),
                Email = "someone@example.com",
                DateOfBirth = DateTime.Now,
                Name = "R2-D2",
                Created = created,
                Modified = created,
            };

            var account2 = new Account()
            {
                Id = Guid.Parse("4bfc0a4a-8e1d-4f10-80b1-b3d6f3cb60b1"),
                Email = "someone@example.com",
                DateOfBirth = DateTime.Now,
                Name = "C3PO",
                Created = created,
                Modified = created,
            };

            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Locations.Add(location3);
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