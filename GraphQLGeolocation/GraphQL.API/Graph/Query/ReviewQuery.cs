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
    public class ReviewQuery : IFieldQueryServiceItem
    {
        public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
        {
            objectGraph.Field<ListGraphType<ReviewType>>("reviews",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> {Name = "description"}
                ),
                resolve: context =>
                {
                    var reviewRepository = (IGenericRepository<Review>) sp.GetService(typeof(IGenericRepository<Review>));
                    var baseQuery = reviewRepository.GetAll();
                    var description = context.GetArgument<string>("description");
                    return description != default(string) ? baseQuery.Where(w => w.Description.Contains(description)) : baseQuery.ToList();
                });
        }
    }
}