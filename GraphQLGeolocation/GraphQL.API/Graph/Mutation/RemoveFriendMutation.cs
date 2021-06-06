using System;
using System.Linq;
using GraphQL.API.Graph.Type;
using GraphQL.API.Interfaces;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.API.Graph.Mutation
{
    public class RemoveFriendMutation : IFieldMutationServiceItem
    {
        public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
        {
            objectGraph.Field<AccountType>("removeFriend",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "sourceId"},
                    new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "destinationId"}
                ),
                resolve: context =>
                {
                    var sourceId = context.GetArgument<int>("sourceId");
                    var destinationId = context.GetArgument<int>("destinationId");

                    var accountRepository =
                        (IGenericRepository<Account>) sp.GetService(typeof(IGenericRepository<Account>));
                    var sourceAccount = accountRepository.GetById(sourceId);
                    var destinationAccount = accountRepository.GetById(destinationId);

                    if (sourceAccount != null && destinationAccount != null)
                    {
                        sourceAccount.Friends = sourceAccount.Friends
                            .Where(a => a.Id != destinationId).ToList();
                        destinationAccount.Friends = destinationAccount.Friends
                            .Where(a => a.Id != sourceId).ToList();
                    }

                    accountRepository.Update(destinationAccount);
                    return accountRepository.Update(sourceAccount);
                });
        }
    }
}