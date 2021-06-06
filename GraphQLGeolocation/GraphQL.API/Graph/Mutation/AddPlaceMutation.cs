using System;
using GraphQL.API.Graph.Type;
using GraphQL.API.Interfaces;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;
using Microsoft.AspNetCore.Hosting;

namespace GraphQL.API.Graph.Mutation
{
    public class AddPlaceMutation : IFieldMutationServiceItem
    {
        public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
        {
            objectGraph.Field<PlaceType>("addPlace",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> {Name = "cityId"},
                    new QueryArgument<NonNullGraphType<StringGraphType>> {Name = "placeName"},
                    new QueryArgument<FloatGraphType> {Name = "latitude"},
                    new QueryArgument<FloatGraphType> {Name = "longitude"}
                ),
                resolve: context =>
                {
                    var placeName = context.GetArgument<string>("placeName");
                    var cityId = context.GetArgument<int>("cityId");
                    var latitude = context.GetArgument<double>("latitude");
                    var longitude = context.GetArgument<double>("longitude");
                    
                    var placeRepository = (IGenericRepository<Place>) sp.GetService(typeof(IGenericRepository<Place>));
                    var locationRepository = (IGenericRepository<Location>) sp.GetService(typeof(IGenericRepository<Location>));

                    var newLocation = new Location
                    {
                        Latitude = latitude,
                        Longitude = longitude
                    };

                    var addedLocation = locationRepository.Insert(newLocation);

                    var newPlace = new Place
                    {
                        Name = placeName,
                        CityId = cityId,
                        LocationId = addedLocation.Id,
                        Location = addedLocation
                    };

                    var addedPlace = placeRepository.Insert(newPlace);

                    return addedPlace;

                });
        }
    }
}