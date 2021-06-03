using System;
using GraphQL.API.Graph.Type;
using GraphQL.API.Interfaces;
using GraphQL.API.Messages;
using GraphQL.Resolvers;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.API.Graph.Subscription
{
    public class CityAddedSubscription : IFieldSubscriptionServiceItem
    {
        public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
        {
            objectGraph.AddField(new EventStreamFieldType
            {
                Name = "cityAdded",
                Type = typeof(CityAddedMessageType),
                Resolver = new FuncFieldResolver<CityAddedMessage>(context => context.Source as CityAddedMessage),
                Arguments = new QueryArguments(                   
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "countryName" }
                ),
                Subscriber = new EventStreamResolver<CityAddedMessage>(context =>
                {
                    var subscriptionServices = (ISubscriptionServices)sp.GetService(typeof(ISubscriptionServices));
                    var countryName = context.GetArgument<string>("countryName");                  
                    return subscriptionServices.CityAddedService.GetMessages(countryName);
                })
            });
        }
    }
}