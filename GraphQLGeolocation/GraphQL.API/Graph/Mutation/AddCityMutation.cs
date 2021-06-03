using System;
using GraphQL.API.Graph.Type;
using GraphQL.API.Interfaces;
using GraphQL.API.Messages;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.API.Graph.Mutation
{
    public class AddCityMutation : IFieldMutationServiceItem
    {
        public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
        {
            objectGraph.Field<CityType>("addCity",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "countryId"},
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "cityName"},
                    new QueryArgument<IntGraphType> {Name = "population"}
                ),
                resolve: context =>
                {
                    var countryId = context.GetArgument<int>("countryId");
                    var cityName = context.GetArgument<string>("cityName");
                    var population = context.GetArgument<int?>("population");

                    var subscriptionServices = (ISubscriptionServices) sp.GetService(typeof(ISubscriptionServices));
                    var cityRepository = (IGenericRepository<City>) sp.GetService(typeof(IGenericRepository<City>));
                    var countryRepository =
                        (IGenericRepository<Country>) sp.GetService(typeof(IGenericRepository<Country>));

                    var foundCountry = countryRepository.GetById(countryId);

                    var newCity = new City
                    {
                        Name = cityName,
                        CountryId = countryId,
                        Population = population
                    };

                    var addedCity = cityRepository.Insert(newCity);
                    subscriptionServices.CityAddedService.AddCityAddedMessage(new CityAddedMessage
                    {
                        CityName = addedCity.Name,
                        CountryName = foundCountry.Name,
                        Id = addedCity.Id,
                        Message = "A new city added"
                    });
                    return addedCity;

                });
        }
    }
}