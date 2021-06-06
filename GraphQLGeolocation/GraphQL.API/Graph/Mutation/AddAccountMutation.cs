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
    public class AddAccountMutation : IFieldMutationServiceItem
    {
        public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
        {
            objectGraph.Field<AccountType>("addAccount",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "name"},
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "email"},
                    new QueryArgument<NonNullGraphType<DateTimeGraphType>> {Name = "dateOfBirth"}
                ),
                resolve: context =>
                {
                    var name = context.GetArgument<string>("name");
                    var email = context.GetArgument<string>("email");
                    var dateOfBirth = context.GetArgument<DateTime>("dateOfBirth");
                    
                    var accountRepository = (IGenericRepository<Account>) sp.GetService(typeof(IGenericRepository<Account>));

                    var newAccount = new Account
                    {
                        Name = name,
                        Email = email,
                        DateOfBirth = dateOfBirth,
                        Friends = new List<Account>(),
                        Reviews = new List<Review>()
                    };

                    var addedAccount = accountRepository.Insert(newAccount);

                    return addedAccount;

                });
        }
    }
}