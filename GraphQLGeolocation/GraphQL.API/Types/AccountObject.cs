using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;

namespace GraphQL.API.Types
{
    public class AccountObject : ObjectGraphType<Account>
    {
        public AccountObject(IAccountRepository accountRepository)
        {
            Name = "Account";
            Description = "Account instance - every account is meant to share multiple location tags with friends";
            
            Field(x => x.Id, type: typeof(NonNullGraphType<IdGraphType>))
                .Description("Unique identifier of the human");
            
            Field(x => x.Name)
                .Description("The name of the human");
            
            Field(x => x.DateOfBirth)
                .Description("The humans date of birth");

            Field(x => x.HomePlanet, nullable: true)
                .Description("The home planet of the human");
            
            Field(x => x.AppearsIn, type: typeof(ListGraphType<EpisodeEnumeration>))
                .Description("List of movies the human appears in");
            
            FieldAsync<ListGraphType<CharacterInterface>, List<Character>>(
                nameof(Human.Friends),
                "List of friends of the human",
                resolve: context => humanRepository.GetFriendsAsync(context.Source, context.CancellationToken));
        }
    }
}