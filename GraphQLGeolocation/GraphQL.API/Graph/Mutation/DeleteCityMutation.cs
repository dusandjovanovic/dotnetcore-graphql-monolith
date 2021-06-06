using System;
using GraphQL.API.Interfaces;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.API.Graph.Mutation
{
    public class DeleteCityMutation : IFieldMutationServiceItem
    {
        public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
        {
            objectGraph.Field<StringGraphType>("deleteCity",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "cityId" }               
                ),
                resolve: context =>
                {
                    var cityId = context.GetArgument<int>("cityId");
                    var cityRepository = (IGenericRepository<City>)sp.GetService(typeof(IGenericRepository<City>));
                    cityRepository.Delete(cityId);
                    return $"cityId:{cityId} deleted";
                });
        }
    }
}