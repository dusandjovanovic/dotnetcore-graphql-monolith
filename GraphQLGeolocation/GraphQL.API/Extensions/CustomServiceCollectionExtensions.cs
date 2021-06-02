using System.IO.Compression;
using System.Linq;
using Boxed.AspNetCore;
using GraphQL.API.Auth;
using GraphQL.API.Constants;
using GraphQL.API.Helpers;
using GraphQL.API.Options;
using GraphQL.Data.Context;
using GraphQL.Server;
using GraphQL.Server.Internal;
using GraphQL.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GraphQL.API.Extensions
{
    internal static class CustomServiceCollectionExtensions
    {
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
                .AddUserContextBuilder<GraphQLUserContextBuilder>()
                .AddDataLoader()
                .AddWebSockets()
                .Services
                .AddTransient(typeof(IGraphQLExecuter<>), typeof(InstrumentingGraphQLExecutor<>));
        
        public static IServiceCollection AddCustomGraphQLAuthorization(
            this IServiceCollection services, 
            IConfiguration configuration, 
            IWebHostEnvironment webHostEnvironment) => 
            services
                .AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.Audience = "graphql";
                })
                .Services;
        
        public static IServiceCollection AddCustomGraphQLAuthorizationValidation(
            this IServiceCollection services, 
            IConfiguration configuration, 
            IWebHostEnvironment webHostEnvironment) => 
            services
                .AddSingleton<IValidationRule, AuthValidationRule>();
    }
}