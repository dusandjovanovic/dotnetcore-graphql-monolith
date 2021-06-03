using System;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Graph.Type
{
    public class LocationType :  ObjectGraphType<Location>
    {
        public IServiceProvider Provider { get; set; }
        
        public LocationType(IServiceProvider provider)
        {
            Field(x => x.Id, type: typeof(IntGraphType));
            Field(x => x.Latitude, type: typeof(FloatGraphType));
            Field(x => x.Longitude, type: typeof(FloatGraphType));
        }
    }
}