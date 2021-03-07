using GraphQL.Types;

namespace GraphQL.API.Types.Account
{
    public class AccountInputObject : InputObjectGraphType<Core.Models.Account>
    {
        public AccountInputObject()
        {
            Name = "AccountInput";
            Description = "An account";

            Field(x => x.Name)
                .Description("The name of the account");
            
            Field(x => x.DateOfBirth)
                .Description("The accounts date of birth");
            
            Field(x => x.Email)
                .Description("The email of the account");
        }
    }
}