using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Boxed.AspNetCore;
using GraphQL.API.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

namespace GraphQL.API
{
    public class Program
    {
        public static Task<int> Main(string[] args) => LogAndRunAsync(CreateHostBuilder(args).Build());

        public static async Task<int> LogAndRunAsync(IHost host)
        {
            if (host is null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            var hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();
            hostEnvironment.ApplicationName = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>().Product;

            Log.Logger = CreateLogger(host);

            try
            {
                Log.Information(
                    "Started {Application} in {Environment} mode.",
                    hostEnvironment.ApplicationName,
                    hostEnvironment.EnvironmentName);
                await host.RunAsync().ConfigureAwait(false);
                Log.Information(
                    "Stopped {Application} in {Environment} mode.",
                    hostEnvironment.ApplicationName,
                    hostEnvironment.EnvironmentName);
                return 0;
            }
            catch (Exception exception)
            {
                Log.Fatal(
                    exception,
                    "{Application} terminated unexpectedly in {Environment} mode.",
                    hostEnvironment.ApplicationName,
                    hostEnvironment.EnvironmentName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureHostConfiguration(
                    configurationBuilder => configurationBuilder
                        .AddEnvironmentVariables(prefix: "DOTNET_")
                        .AddIf(
                            args is not null,
                            x => x.AddCommandLine(args)))
                .ConfigureAppConfiguration((hostingContext, config) =>
                    AddConfiguration(config, hostingContext.HostingEnvironment, args))
                .UseSerilog()
                .UseDefaultServiceProvider(
                    (context, options) =>
                    {
                        var isDevelopment = context.HostingEnvironment.IsDevelopment();
                        options.ValidateScopes = isDevelopment;
                        options.ValidateOnBuild = isDevelopment;
                    })
                .ConfigureWebHost(ConfigureWebHostBuilder)
                .UseConsoleLifetime();

        private static void ConfigureWebHostBuilder(IWebHostBuilder webHostBuilder) =>
            webHostBuilder
                .UseKestrel(
                    (builderContext, options) =>
                    {
                        options.AddServerHeader = false;
                        options.AllowSynchronousIO = true;
                        options.Configure(builderContext.Configuration.GetSection(nameof(ApplicationOptions.Kestrel)), reloadOnChange: false);
                    })
                .UseIIS()
                .ConfigureServices(
                    services => services.Configure<IISServerOptions>(options => options.AllowSynchronousIO = true))
                .UseStartup<Startup>();

        private static IConfigurationBuilder AddConfiguration(
            IConfigurationBuilder configurationBuilder,
            IHostEnvironment hostEnvironment,
            string[] args) =>
            configurationBuilder
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: false)
                .AddKeyPerFile(Path.Combine(Directory.GetCurrentDirectory(), "configuration"), optional: true, reloadOnChange: false)
                .AddIf(
                    hostEnvironment.IsDevelopment() && !string.IsNullOrEmpty(hostEnvironment.ApplicationName),
                    x => x.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true, reloadOnChange: false))
                .AddEnvironmentVariables()
                .AddIf(
                    args is not null,
                    x => x.AddCommandLine(args));

        private static Logger CreateLogger(IHost host)
        {
            var hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();
            return new LoggerConfiguration()
                .ReadFrom.Configuration(host.Services.GetRequiredService<IConfiguration>())
                .Enrich.WithProperty("Application", hostEnvironment.ApplicationName)
                .Enrich.WithProperty("Environment", hostEnvironment.EnvironmentName)
                .WriteTo.Conditional(
                    x => hostEnvironment.IsDevelopment(),
                    x => x.Console().WriteTo.Debug())
                .CreateLogger();
        }
    }
}