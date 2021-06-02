using System.Security.Claims;
using GraphQL.Validation;

namespace GraphQL.API.Auth
{
    public class AuthValidationRule : IValidationRule
    {
        public INodeVisitor Validate(ValidationContext context)
        {
            var userContext = context.UserContext as ClaimsPrincipal;
            var authenticated = userContext?.Identity?.IsAuthenticated ?? false;
            
            return new EnterLeaveListener(_ =>
            {
                if (!authenticated)
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