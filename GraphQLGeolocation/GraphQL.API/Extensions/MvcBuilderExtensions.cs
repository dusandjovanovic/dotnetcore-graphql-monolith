using GraphQL.API.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GraphQL.API.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddCustomJsonOptions(
            this IMvcBuilder builder,
            IWebHostEnvironment webHostEnvironment) =>
            builder.AddNewtonsoftJson(
                options =>
                {
                    if (webHostEnvironment.IsDevelopment())
                    {
                        options.SerializerSettings.Formatting = Formatting.Indented;
                    }
                    
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                    
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

        public static IMvcBuilder AddCustomMvcOptions(this IMvcBuilder builder, IConfiguration configuration) =>
            builder.AddMvcOptions(
                options =>
                {
                    foreach (var keyValuePair in configuration
                        .GetSection(nameof(ApplicationOptions.CacheProfiles))
                        .Get<CacheProfileOptions>())
                    {
                        options.CacheProfiles.Add(keyValuePair);
                    }
                    
                    options.OutputFormatters.RemoveType<StringOutputFormatter>();
                    
                    options.ReturnHttpNotAcceptable = true;
                });
    }
}