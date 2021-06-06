using System;
using GraphQL.API.Graph.Type;
using GraphQL.API.Interfaces;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.API.Graph.Mutation
{
    public class AddReviewMutation : IFieldMutationServiceItem
    {
        public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
        {
            objectGraph.Field<ReviewType>("addReview",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "description"},
                    new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "placeId"},
                    new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "accountId"}
                ),
                resolve: context =>
                {
                    var description = context.GetArgument<string>("description");
                    var placeId = context.GetArgument<int>("placeId");
                    var accountId = context.GetArgument<int>("accountId");
                    
                    var accountRepository = (IGenericRepository<Account>) sp.GetService(typeof(IGenericRepository<Account>));
                    var placeRepository = (IGenericRepository<Place>) sp.GetService(typeof(IGenericRepository<Place>));
                    var reviewRepository = (IGenericRepository<Review>) sp.GetService(typeof(IGenericRepository<Review>));
                    
                    var foundPlace = placeRepository.GetById(placeId);
                    var foundAccount = accountRepository.GetById(accountId);

                    var newReview = new Review
                    {
                        Description = description,
                        Place = foundPlace,
                        Account = foundAccount,
                        PlaceId = placeId,
                        AccountId = accountId
                    };

                    var addReview = reviewRepository.Insert(newReview);
                    foundAccount.Reviews.Add(addReview);
                    accountRepository.Update(foundAccount);

                    return addReview;
                });
        }
    }
}