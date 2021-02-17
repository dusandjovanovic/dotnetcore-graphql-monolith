using GraphQL.API.Helpers;
using GraphQL.Types;

namespace GraphQL.API.Models
{
    public class StatsSchema : Schema
    {
        public StatsSchema(ContextServiceLocator contextServiceLocator) : base()
        {
            Query = new StatsQuery(contextServiceLocator);
            Mutation = new StatsMutation(contextServiceLocator);
        }
    }
}