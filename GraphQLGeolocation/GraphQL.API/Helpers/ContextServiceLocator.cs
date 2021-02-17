using GraphQL.Core.Data;
using GraphQL.Utilities;
using Microsoft.AspNetCore.Http;

namespace GraphQL.API.Helpers
{
    public class ContextServiceLocator
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextServiceLocator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IPlayerRepository PlayerRepository =>
            _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IPlayerRepository>();

        public ISkaterStatisticRepository SkaterStatisticRepository => _httpContextAccessor.HttpContext.RequestServices
            .GetRequiredService<ISkaterStatisticRepository>();
    }
}