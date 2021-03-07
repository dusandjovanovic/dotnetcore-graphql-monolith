using GraphQL.Types;

namespace GraphQL.API.Types.Place
{
    public class PlaceInputObject : InputObjectGraphType<Core.Models.Place>
    {
        public PlaceInputObject()
        {
            Name = "PlaceInput";
            Description = "A place";

            Field(x => x.Name)
                .Description("The name of the place");
            
            Field(x => x.Location)
                .Description("The places location");
        }
    }
}