using GraphQL.API.Types.Place;
using GraphQL.Core.Data;

namespace GraphQL.API.Types
{
    public class PlaceCreatedEvent : PlaceObject
    {
        public PlaceCreatedEvent(IPlaceRepository placeRepository) 
            : base(placeRepository) { }
    }
}