using GraphQL.Types;

namespace GraphQL.API.Types.Location
{
    public class LocationObject : ObjectGraphType<Core.Models.Location>
    {
        public LocationObject()
        {
            Name = "Location";
            Description = "Location instance - latitude/longitude coordinates";

            Field(x => x.Latitude)
                .Description("The name of the place");
            
            Field(x => x.Longitude)
                .Description("The location of the place");
        }
    }
}