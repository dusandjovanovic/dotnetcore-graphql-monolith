using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Data.Repositories;
using GraphQL.Instrumentation;
using GraphQL.Types;

namespace GraphQL.API.Graph.Type
{
    public class AccountType : ObjectGraphType<Account>
    {
        public IServiceProvider Provider { get; set; }
        
        public AccountType(IServiceProvider provider)
        {
            Field(x => x.Id, type: typeof(IntGraphType));
            Field(x => x.Name, type: typeof(StringGraphType));
            Field(x => x.Email, type: typeof(StringGraphType));
            Field(x => x.DateOfBirth, type: typeof(DateTimeGraphType));
            FieldAsync<ListGraphType<AccountType>, IEnumerable<Account>>(
                "friends", 
                resolve: context => {
                    AccountRepository accountRepository = (AccountRepository)provider.GetService(typeof(AccountRepository));
                    return accountRepository.GetFriends(context.Source.Id);
                });
            FieldAsync<ListGraphType<ReviewType>, IEnumerable<Review>>(
                "reviews", 
                resolve: context => {
                    ReviewRepository reviewRepository = (ReviewRepository)provider.GetService(typeof(ReviewRepository));
                    return reviewRepository.GetFromAccount(context.Source.Id);
                });
        }
    }
}