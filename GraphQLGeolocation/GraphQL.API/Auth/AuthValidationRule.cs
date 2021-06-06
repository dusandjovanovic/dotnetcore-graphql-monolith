using System.Net.Http;
using System.Security.Claims;
using GraphQL.Validation;
using Microsoft.AspNetCore.Http;

namespace GraphQL.API.Auth
{
    public class AuthValidationRule : IValidationRule
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public AuthValidationRule(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public INodeVisitor Validate(ValidationContext context)
        {
            return new EnterLeaveListener(_ =>
            {
                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    context.ReportError(new ValidationError(
                        context.OriginalQuery,
                        "authentication-required",
                        "Api can be accessed only by authenticated users"
                    ));
                }
            });
        }
    }
}