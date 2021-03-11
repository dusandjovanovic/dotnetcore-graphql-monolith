using GraphQL.Core.Models;
using GraphQL.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Data.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}
        
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(c => c.Id);
            modelBuilder.Entity<Place>().HasKey(c => c.Id);
            modelBuilder.Entity<Tag>().HasKey(c => c.Id);
            modelBuilder.Entity<Location>().HasKey(c => c.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}