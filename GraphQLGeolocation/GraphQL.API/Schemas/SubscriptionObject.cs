using System.Collections.Generic;
using System.Reactive.Linq;
using GraphQL.API.Types;
using GraphQL.API.Types.Account;
using GraphQL.API.Types.Tag;
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
        public SubscriptionObject(IAccountRepository accountRepository, IPlaceRepository placeRepository,
            ITagRepository tagRepository)
        {
            Name = "Subscription";
            Description = "The subscription type, represents all updates can be pushed to the client in real time over web sockets";

            AddField(
                new EventStreamFieldType()
                {
                    Name = "accountCreated",
                    Description = "Subscribe to account created events.",
                    Arguments = new QueryArguments(
                        new QueryArgument<ListGraphType<StringGraphType>>()
                        {
                            Name = "accountCreated",
                        }),
                    Type = typeof(AccountCreatedEvent),
                    Resolver = new FuncFieldResolver<Account>(context => context.Source as Account),
                    Subscriber = new EventStreamResolver<Account>(context => accountRepository.WhenAccountCreated),
                });
            
            AddField(
                new EventStreamFieldType()
                {
                    Name = "placeCreated",
                    Description = "Subscribe to place created events.",
                    Arguments = new QueryArguments(
                        new QueryArgument<ListGraphType<StringGraphType>>()
                        {
                            Name = "placeCreated",
                        }),
                    Type = typeof(PlaceCreatedEvent),
                    Resolver = new FuncFieldResolver<Place>(context => context.Source as Place),
                    Subscriber = new EventStreamResolver<Place>(context => placeRepository.WhenPlaceCreated),
                });
            
            AddField(
                new EventStreamFieldType()
                {
                    Name = "tagCreated",
                    Description = "Subscribe to tag created events.",
                    Arguments = new QueryArguments(
                        new QueryArgument<ListGraphType<StringGraphType>>()
                        {
                            Name = "tagCreated",
                        }),
                    Type = typeof(TagCreatedEvent),
                    Resolver = new FuncFieldResolver<Tag>(context => context.Source as Tag),
                    Subscriber = new EventStreamResolver<Tag>(context => tagRepository.WhenTagCreated),
                });
        }
    }
}