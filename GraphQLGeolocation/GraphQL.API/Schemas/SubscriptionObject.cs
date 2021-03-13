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
        }
    }
}