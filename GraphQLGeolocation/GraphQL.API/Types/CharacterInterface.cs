using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Types
{
    public class CharacterInterface : InterfaceGraphType<Character>
    {
        public CharacterInterface()
        {
            Name = "Character";
            Description = "A character description";
            
            Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>))
                .Description("Unique character identifier");
            
            Field(x => x.Name, nullable: true)
                .Description("Character name");
            
            Field(x => x.AppearsIn, type: typeof(ListGraphType<EpisodeEnumeration>))
                .Description("List of movies character appears in");
            
            Field<ListGraphType<CharacterInterface>>(nameof(Character.Friends), "List of character's friends");
        }
    }
}