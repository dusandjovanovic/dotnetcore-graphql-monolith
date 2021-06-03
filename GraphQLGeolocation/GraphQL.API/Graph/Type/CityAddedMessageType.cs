using GraphQL.API.Messages;
using GraphQL.Types;

namespace GraphQL.API.Graph.Type
{
    public class CityAddedMessageType : ObjectGraphType<CityAddedMessage>
    {
        public CityAddedMessageType()
        {
            Field(x => x.Id, type: typeof(IntGraphType));
            Field(x => x.Message, type: typeof(StringGraphType));
            Field(x => x.CityName, type: typeof(StringGraphType));
            Field(x => x.CountryName, type: typeof(StringGraphType));
        }
    }
}