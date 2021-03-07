using GraphQL.API.Types.Place;
using GraphQL.API.Types.Tag;
using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Types.Account
{
    public class AccountInterface : InterfaceGraphType<Core.Models.Account>
    {
        public AccountInterface()
        {
            Name = "Account";
            Description = "An account description";
            
            Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>))
                .Description("Unique character identifier");
            
            Field(x => x.Name, nullable: true)
                .Description("Account name");
            
            Field<ListGraphType<AccountInterface>>(nameof(Core.Models.Account.Friends), "List of account's friends");
            
            Field<ListGraphType<TagInterface>>(nameof(Core.Models.Account.SharedTags), "List of account's tags");
            
            Field<ListGraphType<PlaceInterface>>(nameof(Core.Models.Account.AppearsIn), "List of account's places");
        }
    }
}