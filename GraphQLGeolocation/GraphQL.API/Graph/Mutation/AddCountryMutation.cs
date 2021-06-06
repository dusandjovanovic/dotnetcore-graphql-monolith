using System;
using GraphQL.API.Graph.Type;
using GraphQL.API.Interfaces;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.API.Graph.Mutation
{
    public class AddCountryMutation : IFieldMutationServiceItem
    {
        public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
        {
            objectGraph.Field<CountryType>("addCountry",
                arguments: new QueryArguments(               
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "countryName" }
                ),
                resolve: context =>
                {                
                    var countryName = context.GetArgument<string>("countryName");
                    var countryRepository = (IGenericRepository<Country>)sp.GetService(typeof(IGenericRepository<Country>));

                    var newCountry = new Country
                    {
                        Name = countryName
                    };

                    return countryRepository.Insert(newCountry);
                });
        }
    }
}