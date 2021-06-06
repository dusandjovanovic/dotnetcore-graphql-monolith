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
    public class CityQuery : IFieldQueryServiceItem
    {
        public void Activate(ObjectGraphType objectGraph, IWebHostEnvironment env, IServiceProvider sp)
        {
            objectGraph.Field<ListGraphType<CityType>>("cities",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> {Name = "name"}
                ),
                resolve: context =>
                {
                    var cityRepository = (IGenericRepository<City>) sp.GetService(typeof(IGenericRepository<City>));
                    var baseQuery = cityRepository.GetAll();
                    var name = context.GetArgument<string>("name");
                    return name != default(string) ? baseQuery.Where(w => w.Name.Contains(name)) : baseQuery.ToList();
                });
        }
    }
}