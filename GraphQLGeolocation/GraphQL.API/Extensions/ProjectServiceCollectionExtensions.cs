using GraphQL.API.Schemas;
using GraphQL.Core.Data;
using GraphQL.Data.Repositories;
using GraphQL.Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.API.Extensions
{
    internal static class ProjectServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
            services
                .AddSingleton<IClockService, ClockService>();
        
        public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
            services
                .AddScoped<IAccountRepository, AccountRepository>()
                .AddScoped<ITagRepository, TagRepository>()
                .AddScoped<IPlaceRepository, PlaceRepository>();
        
        public static IServiceCollection AddProjectSchemas(this IServiceCollection services) =>
            services
                .AddScoped<MainSchema>();
    }
}