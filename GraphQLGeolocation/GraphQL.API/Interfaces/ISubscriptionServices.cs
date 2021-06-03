using GraphQL.API.Services;

namespace GraphQL.API.Interfaces
{
    public interface ISubscriptionServices
    {
        CityAddedService CityAddedService { get; }
    }
}