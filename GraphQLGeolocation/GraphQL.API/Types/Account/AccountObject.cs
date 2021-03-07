using System.Collections.Generic;
using GraphQL.API.Types.Place;
using GraphQL.API.Types.Tag;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Types.Account
{
    public class AccountObject : ObjectGraphType<Core.Models.Account>
    {
        public AccountObject(IAccountRepository accountRepository)
        {
            Name = "Account";
            Description = "Account instance - every account is meant to share multiple location tags with friends";
            
            Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>))
                .Description("Unique identifier of the account");
            
            Field(x => x.Name)
                .Description("The name of the account");
            
            Field(x => x.DateOfBirth)
                .Description("The accounts date of birth");

            FieldAsync<ListGraphType<AccountInterface>, List<Core.Models.Account>>(
                nameof(Core.Models.Account.Friends),
                "List of friends of the account",
                resolve: context => accountRepository.GetFriendsAsync(context.Source, context.CancellationToken));
            
            FieldAsync<ListGraphType<PlaceInterface>, List<Core.Models.Place>>(
                nameof(Core.Models.Account.AppearsIn),
                "List of places the account appears in",
                resolve: context => accountRepository.GetAppearsInAsync(context.Source, context.CancellationToken));
            
            FieldAsync<ListGraphType<TagInterface>, List<Core.Models.Tag>>(
                nameof(Core.Models.Account.SharedTags),
                "List of tags the account has shared",
                resolve: context => accountRepository.GetSharedTagsAsync(context.Source, context.CancellationToken));

            Interface<AccountInterface>();
        }
    }
}