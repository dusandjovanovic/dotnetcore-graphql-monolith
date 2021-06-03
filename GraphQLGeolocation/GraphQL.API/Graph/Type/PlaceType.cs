using System;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Graph.Type
{
    public class PlaceType : ObjectGraphType<Place>
    {
        public IServiceProvider Provider { get; set; }
        
        public PlaceType(IServiceProvider provider)
        {
            Field(x => x.Id, type: typeof(IntGraphType));
            Field(x => x.Name, type: typeof(StringGraphType));
            Field<LocationType>("location", resolve: context => {
                IGenericRepository<Location> locationRepository = (IGenericRepository<Location>)provider.GetService(typeof(IGenericRepository<Location>));
                return locationRepository.GetById(context.Source.LocationId);
            });
            Field<CountryType>("city", resolve: context => {
                IGenericRepository<City> cityRepository = (IGenericRepository<City>)provider.GetService(typeof(IGenericRepository<City>));
                return cityRepository.GetById(context.Source.CityId);
            });
        }
    }
}