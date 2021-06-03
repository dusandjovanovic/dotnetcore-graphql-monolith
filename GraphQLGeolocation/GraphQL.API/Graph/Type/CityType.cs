using System;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Graph.Type
{
    public class CityType : ObjectGraphType<City>
    {
        public IServiceProvider Provider { get; set; }
        public CityType(IServiceProvider provider)
        {
            Field(x => x.Id, type: typeof(IntGraphType));
            Field(x => x.Name, type: typeof(StringGraphType));
            Field(x => x.Population, type: typeof(IntGraphType));
            Field<CountryType>("country", resolve: context => {
                IGenericRepository<Country> countryRepository = (IGenericRepository<Country>)provider.GetService(typeof(IGenericRepository<Country>));
                return countryRepository.GetById(context.Source.CountryId);
            });
        }
    }
}