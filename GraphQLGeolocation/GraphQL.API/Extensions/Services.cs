using System.IO.Compression;
using System.Linq;
using Boxed.AspNetCore;
using GraphQL.API.Auth;
using GraphQL.API.Constants;
using GraphQL.API.Graph.Mutation;
using GraphQL.API.Graph.Query;
using GraphQL.API.Graph.Schema;
using GraphQL.API.Graph.Subscription;
using GraphQL.API.Graph.Type;
using GraphQL.API.Interfaces;
using GraphQL.API.Options;
using GraphQL.API.Services;
using GraphQL.Core.Data;
using GraphQL.Data.Context;
using GraphQL.Data.Repositories;
using GraphQL.Data.Services;
using GraphQL.Server;
using GraphQL.Server.Internal;
using GraphQL.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Okta.AspNetCore;

namespace GraphQL.API.Extensions
{
    internal static class Services
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
            services
                .AddSingleton<IClockService, ClockService>()
                .AddScoped<IFieldService, FieldService>()
                .AddScoped<IDocumentExecuter, DocumentExecuter>()
                .AddSingleton<ISubscriptionServices, SubscriptionServices>()
                .AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
        
        public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
            services
                .AddScoped<AccountRepository>()
                .AddScoped<ReviewRepository>()
                .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        
        public static IServiceCollection AddProjectSchema(this IServiceCollection services) =>
            services
                .AddScoped<MainMutation>()
                .AddScoped<MainQuery>()
                .AddScoped<CityType>()
                .AddScoped<CountryType>()
                .AddScoped<AccountType>()
                .AddScoped<LocationType>()
                .AddScoped<PlaceType>()
                .AddScoped<ReviewType>()
                .AddScoped<MainSubscription>()
                .AddScoped<GraphQLSchema>();

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddDbContext<ApplicationContext>(options => options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("GraphQL.API")))
                .AddDatabaseDeveloperPageExceptionFilter();
        
        public static IServiceCollection AddCustomCaching(this IServiceCollection services) =>
            services
                .AddMemoryCache()
                .AddDistributedMemoryCache();
        
        public static IServiceCollection AddCustomCors(this IServiceCollection services) =>
            services.AddCors(
                options =>
                    options.AddPolicy(
                        CorsPolicyName.AllowAny,
                        x => x
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()));
        
        public static IServiceCollection AddCustomOptions(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .ConfigureAndValidateSingleton<ApplicationOptions>(configuration)
                .ConfigureAndValidateSingleton<CacheProfileOptions>(configuration.GetSection(nameof(ApplicationOptions.CacheProfiles)))
                .ConfigureAndValidateSingleton<CompressionOptions>(configuration.GetSection(nameof(ApplicationOptions.Compression)))
                .ConfigureAndValidateSingleton<GraphQLOptions>(configuration.GetSection(nameof(ApplicationOptions.GraphQL)))
                .ConfigureAndValidateSingleton<KestrelServerOptions>(configuration.GetSection(nameof(ApplicationOptions.Kestrel)));
        
        public static IServiceCollection AddCustomResponseCompression(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
                .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
                .AddResponseCompression(
                    options =>
                    {
                        var customMimeTypes = configuration
                            .GetSection(nameof(ApplicationOptions.Compression))
                            .Get<CompressionOptions>()
                            ?.MimeTypes ?? Enumerable.Empty<string>();
                        options.MimeTypes = customMimeTypes.Concat(ResponseCompressionDefaults.MimeTypes);

                        options.Providers.Add<BrotliCompressionProvider>();
                        options.Providers.Add<GzipCompressionProvider>();
                    });
        
        public static IServiceCollection AddCustomRouting(this IServiceCollection services) =>
            services.AddRouting(options => options.LowercaseUrls = true);
        
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services) =>
            services
                .AddHealthChecks()
                .Services;

        public static IServiceCollection AddCustomGraphQL(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment) =>
            services
                .AddScoped<IDependencyResolver, GraphQLDependencyResolver>()
                .AddGraphQL(
                    options =>
                    {
                        var graphQLOptions = configuration
                            .GetSection(nameof(ApplicationOptions.GraphQL))
                            .Get<GraphQLOptions>();
                        options.ComplexityConfiguration = graphQLOptions.ComplexityConfiguration;
                        options.EnableMetrics = graphQLOptions.EnableMetrics;
                        options.ExposeExceptions = webHostEnvironment.IsDevelopment();
                    })
                .AddGraphTypes(ServiceLifetime.Scoped)
                .AddRelayGraphTypes()
                .AddDataLoader()
                .AddWebSockets()
                .Services;
        
        public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration) =>
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
                    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
                    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
                })
                .AddOktaWebApi(new OktaWebApiOptions()
                {
                    OktaDomain = configuration
                        .GetSection(nameof(ApplicationOptions.Authentication))
                        .Get<AuthenticationOptions>().Domain
                }).Services;

        public static IServiceCollection AddAuthorizationValidation(
            this IServiceCollection services) => 
            services
                .AddSingleton<IValidationRule, AuthValidationRule>();
    }
}