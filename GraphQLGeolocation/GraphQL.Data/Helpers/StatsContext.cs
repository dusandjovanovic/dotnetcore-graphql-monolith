using GraphQL.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Data.Helpers
{
    public sealed class StatsContext : DbContext
    {
        public StatsContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }
        
        public DbSet<Player> Players { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<SkaterStatistic> SkaterStatistics { get; set; }
    }
}