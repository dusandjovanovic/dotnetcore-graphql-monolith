using Boxed.AspNetCore;
using GraphQL.API.Constants;
using GraphQL.API.Extensions;
using GraphQL.API.Graph.Schema;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GraphQL.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }

        public virtual void ConfigureServices(IServiceCollection services) =>
            services
                .AddDbContext(configuration)
                .AddCustomCaching()
                .AddCustomCors()
                .AddCustomOptions(configuration)
                .AddCustomRouting()
                .AddCustomResponseCompression(configuration)
                .AddCustomHealthChecks()
                .AddHttpContextAccessor()
                .AddServerTiming()
                .AddControllers()
                .AddCustomJsonOptions(webHostEnvironment)
                .AddCustomMvcOptions(configuration)
                .Services
                .AddCustomGraphQL(configuration, webHostEnvironment)
                .AddAuthorization(configuration)
                .AddAuthorizationValidation()
                .AddProjectServices()
                .AddProjectRepositories()
                .AddProjectSchema();

        public virtual void Configure(IApplicationBuilder application) =>
            application
                .UseIf(
                    webHostEnvironment.IsDevelopment(),
                    x => x.UseServerTiming())
                .UseResponseCompression()
                .UseIf(
                    webHostEnvironment.IsDevelopment(),
                    x => x.UseDeveloperExceptionPage())
                .UseRouting()
                .UseCors(CorsPolicyName.AllowAny)
                .UseStaticFilesWithCacheControl()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(
                    builder =>
                    {
                        builder
                            .MapHealthChecks("/status")
                            .RequireCors(CorsPolicyName.AllowAny);
                        builder
                            .MapHealthChecks("/status/self", new HealthCheckOptions() {Predicate = _ => false})
                            .RequireCors(CorsPolicyName.AllowAny);
                    })
                .UseWebSockets()
                .UseGraphQLWebSockets<GraphQLSchema>()
                .UseGraphQL<GraphQLSchema>()
                .UseIf(
                    webHostEnvironment.IsDevelopment(),
                    x => x
                        .UseGraphQLPlayground(new GraphQLPlaygroundOptions() {Path = "/"})
                        .UseGraphQLVoyager(new GraphQLVoyagerOptions() {Path = "/voyager"}));
    }
}