using System;
using System.Collections.Generic;
using GraphQL.API.Graph.Type;
using GraphQL.API.Interfaces;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.API.Graph.Mutation
{
    public class AddFriendMutation : IFieldMutationServiceItem
    {
        public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
        {
            objectGraph.Field<AccountType>("addFriend",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "sourceId"},
                    new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "destinationId"}
                ),
                resolve: context =>
                {
                    var sourceId = context.GetArgument<int>("sourceId");
                    var destinationId = context.GetArgument<int>("destinationId");

                    var accountRepository = (IGenericRepository<Account>)sp.GetService(typeof(IGenericRepository<Account>));
                    var sourceAccount = accountRepository.GetById(sourceId);
                    var destinationAccount = accountRepository.GetById(destinationId);

                    if (sourceAccount != null && destinationAccount != null)
                    {
                        sourceAccount.Friends.Add(destinationAccount);
                        destinationAccount.Friends.Add(sourceAccount);
                    }

                    accountRepository.Update(destinationAccount);
                    return accountRepository.Update(sourceAccount);
                });
        }
    }
}