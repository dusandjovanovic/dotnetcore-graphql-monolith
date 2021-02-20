using System.Security.Claims;
using GraphQL.Authorization;

namespace GraphQL.API.Helpers
{
    public class GraphQLUserContext : IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
    }
}