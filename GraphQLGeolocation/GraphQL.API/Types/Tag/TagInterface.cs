using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Types.Tag
{
    public class TagInterface : InterfaceGraphType<Core.Models.Tag>
    {
        public TagInterface()
        {
            Name = "Tag";
            Description = "A tag description";
            
            Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>))
                .Description("Unique place identifier");
            
            Field(x => x.Description, nullable: true)
                .Description("Tag description");
        }
    }
}