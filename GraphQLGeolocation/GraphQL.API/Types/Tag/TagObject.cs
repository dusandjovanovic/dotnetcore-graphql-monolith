using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Types.Tag
{
    public class TagObject: ObjectGraphType<Core.Models.Tag>
    {
        public TagObject(ITagRepository tagRepository)
        {
            Name = "Tag";
            Description = "Tag instance - every account could share one";
            
            Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>))
                .Description("Unique identifier of the tag");

            Field(x => x.Location)
                .Description("The location of the tag");

            Interface<TagInterface>();
        }
    }
}