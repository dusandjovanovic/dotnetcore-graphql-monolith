using GraphQL.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Data.Factories
{
    public class TemporaryDbContextFactory : DesignTimeDbContextFactoryBase<StatsContext>
    {
        protected override StatsContext CreateNewInstance(DbContextOptions<StatsContext> options)
        {
            return new StatsContext(options);
        }
    }
}