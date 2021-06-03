using GraphQL.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.API.Extensions
{
    internal static class CustomServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddDbContext<ApplicationContext>(options => options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("GraphQL.API")))
                .AddDatabaseDeveloperPageExceptionFilter();
    }
}