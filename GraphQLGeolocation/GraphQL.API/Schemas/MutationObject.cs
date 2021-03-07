using GraphQL.API.Types.Account;
using GraphQL.API.Types.Place;
using GraphQL.API.Types.Tag;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Data.Services;
using GraphQL.Types;

namespace GraphQL.API.Schemas
{
    /**
     * mutation createHuman($human: HumanInput!) {
     *   createHuman(human: $human)
     *   {
     *     id
     *     name
     *     dateOfBirth
     *     appearsIn
     *   }
     * }
     *
     * {
     *  "human": {
     *     "name": "Dushan Jovanovic",
     *     "homePlanet": "Earth",
     *     "dateOfBirth": "2000-01-01",
     *     "appearsIn": [ "NEWHOPE" ]
     *   }
     * }
     */
    public class MutationObject : ObjectGraphType<object>
    {
        public MutationObject(IClockService clockService, IAccountRepository accountRepository, 
            IPlaceRepository placeRepository, ITagRepository tagRepository)
        {
            Name = "Mutation";
            Description = "The mutation type, represents all updates we can make to our data.";

            FieldAsync<AccountObject, Account>(
                "createAccount",
                "Create a new account.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AccountInputObject>>()
                    {
                        Name = "account",
                        Description = "The account you want to create.",
                    }),
                resolve: context =>
                {
                    var account = context.GetArgument<Account>("account");
                    var now = clockService.UtcNow;
                    account.Created = now;
                    account.Modified = now;
                    return accountRepository.AddAccountAsync(account, context.CancellationToken);
                });
            
            FieldAsync<PlaceObject, Place>(
                "createPlace",
                "Create a new place.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<PlaceInputObject>>()
                    {
                        Name = "place",
                        Description = "The place you want to create.",
                    }),
                resolve: context =>
                {
                    var place = context.GetArgument<Place>("place");
                    var now = clockService.UtcNow;
                    place.Created = now;
                    place.Modified = now;
                    return placeRepository.AddPlaceAsync(place, context.CancellationToken);
                });
            
            FieldAsync<TagObject, Tag>(
                "createTag",
                "Create a new tag.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TagInputObject>>()
                    {
                        Name = "tag",
                        Description = "The tag you want to create.",
                    }),
                resolve: context =>
                {
                    var tag = context.GetArgument<Tag>("tag");
                    var now = clockService.UtcNow;
                    tag.Created = now;
                    tag.Modified = now;
                    return tagRepository.AddTagAsync(tag, context.CancellationToken);
                });
        }
    }
}