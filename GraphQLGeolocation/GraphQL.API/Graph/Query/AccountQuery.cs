using System;
using System.Linq;
using GraphQL.API.Graph.Type;
using GraphQL.API.Interfaces;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.API.Graph.Query
{
    public class AccountQuery: IFieldQueryServiceItem
    {
        public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
        {
            objectGraph.Field<ListGraphType<AccountType>>("accounts",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> {Name = "name"}
                ),
                resolve: context =>
                {
                    var accountRepository = (IGenericRepository<Account>) sp.GetService(typeof(IGenericRepository<Account>));
                    var baseQuery = accountRepository.GetAll();
                    var name = context.GetArgument<string>("name");
                    return name != default(string) ? baseQuery.Where(w => w.Name.Contains(name)) : baseQuery.ToList();
                });
        }
    }
}