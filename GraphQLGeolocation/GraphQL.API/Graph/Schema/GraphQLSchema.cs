using GraphQL.API.Graph.Mutation;
using GraphQL.API.Graph.Query;
using GraphQL.API.Graph.Subscription;
using GraphQL.API.Interfaces;

namespace GraphQL.API.Graph.Schema
{
    public class GraphQLSchema : GraphQL.Types.Schema
    {
        public GraphQLSchema(IDependencyResolver resolver) : base(resolver)
        {
            var fieldService = resolver.Resolve<IFieldService>();
            fieldService.RegisterFields();

            Mutation = resolver.Resolve<MainMutation>();
            Query = resolver.Resolve<MainQuery>();
            Subscription = resolver.Resolve<MainSubscription>();
        }
    }
}