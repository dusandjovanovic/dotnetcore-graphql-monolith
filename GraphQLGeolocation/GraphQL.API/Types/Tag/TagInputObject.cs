using GraphQL.API.Types.Location;
using GraphQL.Types;

namespace GraphQL.API.Types.Tag
{
    public class TagInputObject : InputObjectGraphType<Core.Models.Tag>
    {
        public TagInputObject()
        {
            Name = "TagInput";
            Description = "A tag";

            Field(x => x.Description)
                .Description("The name of the place");
            
            Field(x => x.Location, type: typeof(LocationObject))
                .Description("The places location");
        }
    }
}