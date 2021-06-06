using System;
using System.Linq;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Graph.Type
{
    public class CountryType : ObjectGraphType<Country>
    {
        public IServiceProvider Provider { get; set; }
        public CountryType(IServiceProvider provider)
        {
            Field(x => x.Id, type: typeof(IntGraphType));
            Field(x => x.Name, type: typeof(StringGraphType));
            Field<ListGraphType<CityType>>("cities", resolve: context => {
                IGenericRepository<City> cityRepository = (IGenericRepository<City>)provider.GetService(typeof(IGenericRepository<City>));
                return cityRepository.GetAll().Where(w=> w.Country.Id ==  context.Source.Id);
            });
        }
    }
}