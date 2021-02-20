using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Types
{
    public class HumanInputObject : InputObjectGraphType<Human>
    {
        public HumanInputObject()
        {
            Name = "HumanInput";
            Description = "A humanoid creature from the Star Wars universe";

            Field(x => x.Name)
                .Description("The name of the human");
            
            Field(x => x.DateOfBirth)
                .Description("The humans date of birth");
            
            Field(x => x.HomePlanet, nullable: true)
                .Description("The home planet of the human");
            
            Field(x => x.AppearsIn, type: typeof(ListGraphType<EpisodeEnumeration>))
                .Description("List of movies the human appears in");
        }
    }
}