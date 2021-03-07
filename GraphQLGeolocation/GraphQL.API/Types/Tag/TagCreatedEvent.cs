using GraphQL.Core.Data;

namespace GraphQL.API.Types.Tag
{
    public class TagCreatedEvent : TagObject
    {
        public TagCreatedEvent(ITagRepository tagRepository) 
            : base(tagRepository) { }
    }
}