using GraphQL.Types;

namespace GraphQL.API.Schemas
{
    public class MainSchema : Schema
    {
        public MainSchema(QueryObject query, MutationObject mutation, SubscriptionObject subscription,
         IDependencyResolver resolver) 
            : base(resolver)
        {
            Query = query;
            Mutation = mutation;
            Subscription = subscription;
        }
    }
}