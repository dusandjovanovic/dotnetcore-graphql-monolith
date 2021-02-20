using System.Collections.Generic;
using System.Reactive.Linq;
using GraphQL.API.Types;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Resolvers;
using GraphQL.Types;

namespace GraphQL.API.Schemas
{
    /**
    * subscription whenHumanCreated {
    *   humanCreated(homePlanets: ["Earth"])
    *   {
    *     id
    *     name
    *     dateOfBirth
    *     homePlanet
    *     appearsIn
    *   }
    * }
     */
    public class SubscriptionObject : ObjectGraphType<object>
    {
        public SubscriptionObject(IHumanRepository humanRepository)
        {
            Name = "Subscription";
            Description = "The subscription type, represents all updates can be pushed to the client in real time over web sockets";

            AddField(
                new EventStreamFieldType()
                {
                    Name = "humanCreated",
                    Description = "Subscribe to human created events.",
                    Arguments = new QueryArguments(
                        new QueryArgument<ListGraphType<StringGraphType>>()
                        {
                            Name = "homePlanets",
                        }),
                    Type = typeof(HumanCreatedEvent),
                    Resolver = new FuncFieldResolver<Human>(context => context.Source as Human),
                    Subscriber = new EventStreamResolver<Human>(context => humanRepository.WhenHumanCreated),
                });
        }
    }
}