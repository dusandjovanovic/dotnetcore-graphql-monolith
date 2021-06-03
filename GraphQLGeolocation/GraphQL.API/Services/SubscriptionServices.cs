using GraphQL.API.Interfaces;

namespace GraphQL.API.Services
{
    public class SubscriptionServices : ISubscriptionServices
    {
        public SubscriptionServices()
        {
            this.CityAddedService = new CityAddedService();
        }
        public CityAddedService CityAddedService { get; }
    }
}