using System.Collections.Generic;
using GraphQL.API.Types.Tag;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Types.Place
{
    public class PlaceObject : ObjectGraphType<Core.Models.Place>
    {
        public PlaceObject(IPlaceRepository placeRepository)
        {
            Name = "Place";
            Description = "Place instance - every city/village could be added as a place";
            
            Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>))
                .Description("Unique identifier of the place");
            
            Field(x => x.Name)
                .Description("The name of the place");
            
            Field(x => x.Location)
                .Description("The location of the place");
            
            FieldAsync<ListGraphType<TagInterface>, List<Core.Models.Tag>>(
                nameof(Core.Models.Place.Tags),
                "List of tags the place has captured",
                resolve: context => placeRepository.GetTagsAsync(context.Source, context.CancellationToken));

            Interface<PlaceInterface>();
        }
    }
}