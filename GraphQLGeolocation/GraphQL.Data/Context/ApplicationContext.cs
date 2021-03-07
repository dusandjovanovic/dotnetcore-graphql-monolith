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
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountMapping());
            modelBuilder.ApplyConfiguration(new PlaceMapping());
            modelBuilder.ApplyConfiguration(new TagMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}