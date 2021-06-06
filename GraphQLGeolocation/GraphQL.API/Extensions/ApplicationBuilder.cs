using System;
using System.Linq;
using Boxed.AspNetCore;
using GraphQL.API.Constants;
using GraphQL.API.Options;
using GraphQL.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace GraphQL.API.Extensions
{
    internal static partial class ApplicationBuilder
    {
        public static IApplicationBuilder UseStaticFilesWithCacheControl(this IApplicationBuilder application)
        {
            var cacheProfile = application
                .ApplicationServices
                .GetRequiredService<CacheProfileOptions>()
                .Where(x => string.Equals(x.Key, CacheProfileName.StaticFiles, StringComparison.Ordinal))
                .Select(x => x.Value)
                .SingleOrDefault();
            return application
                .UseStaticFiles(
                    new StaticFileOptions()
                    {
                        OnPrepareResponse = context => context.Context.ApplyCacheProfile(cacheProfile),
                    });
        }
        
        public static IApplicationBuilder UseCustomSerilogRequestLogging(this IApplicationBuilder application) =>
            application.UseSerilogRequestLogging(
                options =>
                {
                    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                    {
                        var request = httpContext.Request;
                        var response = httpContext.Response;
                        var endpoint = httpContext.GetEndpoint();

                        diagnosticContext.Set("Host", request.Host);
                        diagnosticContext.Set("Protocol", request.Protocol);
                        diagnosticContext.Set("Scheme", request.Scheme);

                        if (request.QueryString.HasValue)
                        {
                            diagnosticContext.Set("QueryString", request.QueryString.Value);
                        }

                        var clientName = request.Headers["apollographql-client-name"];
                        if (clientName.Any())
                        {
                            diagnosticContext.Set("ClientName", clientName);
                        }

                        var clientVersion = request.Headers["apollographql-client-version"];
                        if (clientVersion.Any())
                        {
                            diagnosticContext.Set("ClientVersion", clientVersion);
                        }

                        if (endpoint is not null)
                        {
                            diagnosticContext.Set("EndpointName", endpoint.DisplayName);
                        }

                        diagnosticContext.Set("ContentType", response.ContentType);
                    };
                    options.GetLevel = GetLevel;

                    static LogEventLevel GetLevel(HttpContext httpContext, double elapsedMilliseconds, Exception exception)
                    {
                        if (exception == null && httpContext.Response.StatusCode <= 499)
                        {
                            if (IsHealthCheckEndpoint(httpContext))
                            {
                                return LogEventLevel.Verbose;
                            }

                            return LogEventLevel.Information;
                        }

                        return LogEventLevel.Error;
                    }

                    static bool IsHealthCheckEndpoint(HttpContext httpContext)
                    {
                        var endpoint = httpContext.GetEndpoint();
                        if (endpoint is not null)
                        {
                            return endpoint.DisplayName == "Health checks";
                        }

                        return false;
                    }
                });
    }
}