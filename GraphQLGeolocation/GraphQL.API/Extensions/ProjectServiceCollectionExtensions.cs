using GraphQL.API.Graph.Mutation;
using GraphQL.API.Graph.Query;
using GraphQL.API.Graph.Schema;
using GraphQL.API.Graph.Subscription;
using GraphQL.API.Graph.Type;
using GraphQL.API.Interfaces;
using GraphQL.API.Services;
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
                .AddSingleton<IClockService, ClockService>()
                .AddScoped<IFieldService, FieldService>()
                .AddScoped<IDocumentExecuter, DocumentExecuter>()
                .AddScoped<MainMutation>()
                .AddScoped<MainQuery>()
                .AddScoped<CityType>()
                .AddScoped<CountryType>()
                .AddScoped<AccountType>()
                .AddScoped<LocationType>()
                .AddScoped<PlaceType>()
                .AddScoped<ReviewType>()
                .AddSingleton<ISubscriptionServices, SubscriptionServices>()
                .AddScoped<MainSubscription>()
                .AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService))
                .AddScoped<GraphQLSchema>();

        public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
            services
                .AddScoped<AccountRepository>()
                .AddScoped<ReviewRepository>()
                .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }
}