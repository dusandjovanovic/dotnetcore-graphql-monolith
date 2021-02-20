using System.Collections.Generic;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Types
{
    public class DroidObject : ObjectGraphType<Droid>
    {
        public DroidObject(IDroidRepository droidRepository)
        {
            Name = "Droid";
            Description = "A mechanical creature in this Universe";
            
            Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>))
                .Description("Unique identifier of the droid");
            
            Field(x => x.Name)
                .Description("Name of the droid");
            
            Field(x => x.ChargePeriod)
                .Description("Charge period of the droid");
            
            Field(x => x.Manufactured)
                .Description("Manufacture date of the droid");
            
            Field(x => x.PrimaryFunction, nullable: true)
                .Description("Primary function of the droid");
            
            Field(x => x.AppearsIn, type: typeof(ListGraphType<EpisodeEnumeration>))
                .Description("List of movies the droid appears in");
            
            FieldAsync<ListGraphType<CharacterInterface>, List<Character>>(
                nameof(Droid.Friends),
                "List of friends of the droid",
                resolve: context => droidRepository.GetFriendsAsync(context.Source, context.CancellationToken));

            Interface<CharacterInterface>();
        }
    }
}