using GraphQL.API.Types.Tag;
using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Types.Place
{
    public class PlaceInterface : InterfaceGraphType<Core.Models.Place>
    {
        public PlaceInterface()
        {
            Name = "Place";
            Description = "A place description";
            
            Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>))
                .Description("Unique place identifier");
            
            Field(x => x.Name, nullable: true)
                .Description("Place name");

            Field<ListGraphType<TagInterface>>(nameof(Core.Models.Place.Tags), "List of place's tags");
        }
    }
}