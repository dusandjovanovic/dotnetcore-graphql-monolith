using System.ComponentModel.DataAnnotations;
using GraphQL.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HostFiltering;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace GraphQL.API.Options
{
    public class ApplicationOptions
    {
        [Required]
        public CacheProfileOptions CacheProfiles { get; }
        
        public CompressionOptions Compression { get; set; }
        
        public ForwardedHeadersOptions ForwardedHeaders { get; set; }
        
        public HostFilteringOptions HostFiltering { get; set; }
        
        public GraphQLOptions GraphQL { get; set; }
        
        public KestrelServerOptions Kestrel { get; set; }
    }
}