using System;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Graph.Type
{
    public class ReviewType : ObjectGraphType<Review>
    {
        public IServiceProvider Provider { get; set; }
        
        public ReviewType(IServiceProvider provider)
        {
            Field(x => x.Id, type: typeof(IntGraphType));
            Field(x => x.Description, type: typeof(StringGraphType));
            Field<LocationType>("account", resolve: context => {
                IGenericRepository<Account> accountRepository = (IGenericRepository<Account>)provider.GetService(typeof(IGenericRepository<Account>));
                return accountRepository.GetById(context.Source.AccountId);
            });
            Field<PlaceType>("place", resolve: context => {
                IGenericRepository<Place> placeRepository = (IGenericRepository<Place>)provider.GetService(typeof(IGenericRepository<Place>));
                return placeRepository.GetById(context.Source.PlaceId);
            });
        }
    }
}