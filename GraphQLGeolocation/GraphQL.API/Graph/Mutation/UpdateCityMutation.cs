using System;
using GraphQL.API.Graph.Type;
using GraphQL.API.Interfaces;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.API.Graph.Mutation
{
    public class UpdateCityMutation : IFieldMutationServiceItem
    {
        public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
        {
            objectGraph.Field<CityType>("updateCity",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "cityId" },
                    new QueryArgument<IntGraphType> { Name = "countryId" },
                    new QueryArgument<StringGraphType> { Name = "cityName" },
                    new QueryArgument<IntGraphType> { Name = "population" }
                ),
                resolve: context =>
                {
                    var cityId = context.GetArgument<int>("cityId");
                    var countryId = context.GetArgument<int?>("countryId");
                    var cityName = context.GetArgument<string>("cityName");
                    var population = context.GetArgument<int?>("population");

                    var cityRepository = (IGenericRepository<City>)sp.GetService(typeof(IGenericRepository<City>));
                    var foundCity = cityRepository.GetById(cityId);

                    if (countryId != null)
                    {
                        foundCity.CountryId = countryId.Value;
                    }
                    if (!string.IsNullOrEmpty(cityName))
                    {
                        foundCity.Name = cityName;
                    }
                    if (population != null)
                    {
                        foundCity.Population = population.Value;
                    }

                    return cityRepository.Update(foundCity);
                });
        }
    }
}