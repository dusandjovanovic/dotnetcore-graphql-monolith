using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.AspNetCore;
using GraphQL.API.Types.Account;
using GraphQL.API.Types.Place;
using GraphQL.API.Types.Tag;
using GraphQL.Builders;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Types;
using GraphQL.Types.Relay.DataObjects;

namespace GraphQL.API.Schemas
{
    /**
     * query getHuman {
     *   human(id: "94fbd693-2027-4804-bf40-ed427fe76fda")
     *   {
     *     id
     *     name
     *     dateOfBirth
     *     homePlanet
     *     appearsIn
     *     friends {
     *       name
     *       ... on Droid {
     *         chargePeriod
     *         created
     *         primaryFunction
     *       }
     *       ... on Human
     *       {
     *         dateOfBirth
     *         homePlanet
     *       }
     *     }
     *   }
     * }
     */
    public class QueryObject : ObjectGraphType<object>
    {
        public QueryObject(IAccountRepository accountRepository, IPlaceRepository placeRepository, 
            ITagRepository tagRepository)
        {
            Name = "Query";
            Description = "The query type, represents all of the entry points into our object graph.";

            FieldAsync<AccountObject, Account>(
                "account",
                "Get an account by its unique identifier.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>
                    {
                        Name = "id",
                        Description = "The unique identifier of the account.",
                    }),
                resolve: context =>
                    accountRepository.GetAccountAsync(
                        context.GetArgument("id", defaultValue: new Guid()),
                        context.CancellationToken));
            
            FieldAsync<PlaceObject, Place>(
                "place",
                "Get a place by its unique identifier.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>()
                    {
                        Name = "id",
                        Description = "The unique identifier of the place.",
                    }),
                resolve: context => placeRepository.GetPlaceAsync(
                    context.GetArgument("id", defaultValue: new Guid()),
                    context.CancellationToken));
            
            FieldAsync<TagObject, Tag>(
                "tag",
                "Get a tag by its unique identifier.",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>>()
                    {
                        Name = "id",
                        Description = "The unique identifier of the tag.",
                    }),
                resolve: context => tagRepository.GetTagAsync(
                    context.GetArgument("id", defaultValue: new Guid()),
                    context.CancellationToken));

            Connection<AccountObject>()
                .Name("accounts")
                .Description("Gets pages of accounts.")
                .Bidirectional()
                .PageSize(MaxPageSize)
                .ResolveAsync(context => ResolveConnectionAsync(accountRepository, context));
            
            Connection<PlaceObject>()
                .Name("places")
                .Description("Gets pages of places.")
                .Bidirectional()
                .PageSize(MaxPageSize)
                .ResolveAsync(context => ResolveConnectionAsync(placeRepository, context));
            
            Connection<TagObject>()
                .Name("tags")
                .Description("Gets pages of tags.")
                .Bidirectional()
                .PageSize(MaxPageSize)
                .ResolveAsync(context => ResolveConnectionAsync(tagRepository, context));
        }
        
        private const int MaxPageSize = 10;
        
        private static async Task<object> ResolveConnectionAsync(IAccountRepository accountRepository, 
            ResolveConnectionContext<object> context)
        {
            var first = context.First;
            var afterCursor = Cursor.FromCursor<DateTime?>(context.After);
            var last = context.Last;
            var beforeCursor = Cursor.FromCursor<DateTime?>(context.Before);
            var cancellationToken = context.CancellationToken;

            var getAccountsTask = GetAccountsAsync(accountRepository, first, afterCursor, last, beforeCursor, cancellationToken);
            var getHasNextPageTask = GetHasNextPageAsync(accountRepository, first, afterCursor, cancellationToken);
            var getHasPreviousPageTask = GetHasPreviousPageAsync(accountRepository, last, beforeCursor, cancellationToken);
            var totalCountTask = accountRepository.GetTotalCountAsync(cancellationToken);

            await Task.WhenAll(getAccountsTask, getHasNextPageTask, getHasPreviousPageTask, totalCountTask).ConfigureAwait(false);
            var accounts = await getAccountsTask.ConfigureAwait(false);
            var hasNextPage = await getHasNextPageTask.ConfigureAwait(false);
            var hasPreviousPage = await getHasPreviousPageTask.ConfigureAwait(false);
            var totalCount = await totalCountTask.ConfigureAwait(false);
            var (firstCursor, lastCursor) = Cursor.GetFirstAndLastCursor(accounts, x => x.Created);

            return new Connection<Account>()
            {
                Edges = accounts
                    .Select(x =>
                        new Edge<Account>()
                        {
                            Cursor = Cursor.ToCursor(x.Created),
                            Node = x,
                        })
                    .ToList(),
                PageInfo = new PageInfo()
                {
                    HasNextPage = hasNextPage,
                    HasPreviousPage = hasPreviousPage,
                    StartCursor = firstCursor,
                    EndCursor = lastCursor,
                },
                TotalCount = totalCount,
            };
        }
        
          private static async Task<object> ResolveConnectionAsync(IPlaceRepository placeRepository, 
            ResolveConnectionContext<object> context)
        {
            var first = context.First;
            var afterCursor = Cursor.FromCursor<DateTime?>(context.After);
            var last = context.Last;
            var beforeCursor = Cursor.FromCursor<DateTime?>(context.Before);
            var cancellationToken = context.CancellationToken;

            var gePlacesTask = GetPlacesAsync(placeRepository, first, afterCursor, last, beforeCursor, cancellationToken);
            var getHasNextPageTask = GetHasNextPageAsync(placeRepository, first, afterCursor, cancellationToken);
            var getHasPreviousPageTask = GetHasPreviousPageAsync(placeRepository, last, beforeCursor, cancellationToken);
            var totalCountTask = placeRepository.GetTotalCountAsync(cancellationToken);

            await Task.WhenAll(gePlacesTask, getHasNextPageTask, getHasPreviousPageTask, totalCountTask).ConfigureAwait(false);
            var accounts = await gePlacesTask.ConfigureAwait(false);
            var hasNextPage = await getHasNextPageTask.ConfigureAwait(false);
            var hasPreviousPage = await getHasPreviousPageTask.ConfigureAwait(false);
            var totalCount = await totalCountTask.ConfigureAwait(false);
            var (firstCursor, lastCursor) = Cursor.GetFirstAndLastCursor(accounts, x => x.Created);

            return new Connection<Place>()
            {
                Edges = accounts
                    .Select(x =>
                        new Edge<Place>()
                        {
                            Cursor = Cursor.ToCursor(x.Created),
                            Node = x,
                        })
                    .ToList(),
                PageInfo = new PageInfo()
                {
                    HasNextPage = hasNextPage,
                    HasPreviousPage = hasPreviousPage,
                    StartCursor = firstCursor,
                    EndCursor = lastCursor,
                },
                TotalCount = totalCount,
            };
        }
          
                private static async Task<object> ResolveConnectionAsync(ITagRepository tagRepository, 
            ResolveConnectionContext<object> context)
        {
            var first = context.First;
            var afterCursor = Cursor.FromCursor<DateTime?>(context.After);
            var last = context.Last;
            var beforeCursor = Cursor.FromCursor<DateTime?>(context.Before);
            var cancellationToken = context.CancellationToken;

            var getTagsTask = GetTagsAsync(tagRepository, first, afterCursor, last, beforeCursor, cancellationToken);
            var getHasNextPageTask = GetHasNextPageAsync(tagRepository, first, afterCursor, cancellationToken);
            var getHasPreviousPageTask = GetHasPreviousPageAsync(tagRepository, last, beforeCursor, cancellationToken);
            var totalCountTask = tagRepository.GetTotalCountAsync(cancellationToken);

            await Task.WhenAll(getTagsTask, getHasNextPageTask, getHasPreviousPageTask, totalCountTask).ConfigureAwait(false);
            var accounts = await getTagsTask.ConfigureAwait(false);
            var hasNextPage = await getHasNextPageTask.ConfigureAwait(false);
            var hasPreviousPage = await getHasPreviousPageTask.ConfigureAwait(false);
            var totalCount = await totalCountTask.ConfigureAwait(false);
            var (firstCursor, lastCursor) = Cursor.GetFirstAndLastCursor(accounts, x => x.Created);

            return new Connection<Tag>()
            {
                Edges = accounts
                    .Select(x =>
                        new Edge<Tag>()
                        {
                            Cursor = Cursor.ToCursor(x.Created),
                            Node = x,
                        })
                    .ToList(),
                PageInfo = new PageInfo()
                {
                    HasNextPage = hasNextPage,
                    HasPreviousPage = hasPreviousPage,
                    StartCursor = firstCursor,
                    EndCursor = lastCursor,
                },
                TotalCount = totalCount,
            };
        }
        
         private static Task<List<Account>> GetAccountsAsync(IAccountRepository accountRepository, int? first, DateTime? afterCursor,
            int? last, DateTime? beforeCursor, CancellationToken cancellationToken)
        {
            Task<List<Account>> getAccountsTask;
            if (first.HasValue)
            {
                getAccountsTask = accountRepository.GetAccountsAsync(first, afterCursor, cancellationToken);
            }
            else
            {
                getAccountsTask = accountRepository.GetAccountsReverseAsync(last, beforeCursor, cancellationToken);
            }

            return getAccountsTask;
        }

        private static Task<bool> GetHasNextPageAsync(IAccountRepository accountRepository, int? first, DateTime? afterCursor,
            CancellationToken cancellationToken)
        {
            return first.HasValue ?
                accountRepository.GetHasNextPageAsync(first, afterCursor, cancellationToken) 
                : Task.FromResult(false);
        }

        private static Task<bool> GetHasPreviousPageAsync(IAccountRepository accountRepository, int? last,
            DateTime? beforeCursor, CancellationToken cancellationToken)
        {
            return last.HasValue 
                ? accountRepository.GetHasPreviousPageAsync(last, beforeCursor, cancellationToken) 
                : Task.FromResult(false);
        }
        
        private static Task<List<Place>> GetPlacesAsync(IPlaceRepository placeRepository, int? first, DateTime? afterCursor,
            int? last, DateTime? beforeCursor, CancellationToken cancellationToken)
        {
            Task<List<Place>> getPlacesTask;
            if (first.HasValue)
            {
                getPlacesTask = placeRepository.GetPlacesAsync(first, afterCursor, cancellationToken);
            }
            else
            {
                getPlacesTask = placeRepository.GetPlacesReverseAsync(last, beforeCursor, cancellationToken);
            }

            return getPlacesTask;
        }

        private static Task<bool> GetHasNextPageAsync(IPlaceRepository placeRepository, int? first, DateTime? afterCursor,
            CancellationToken cancellationToken)
        {
            return first.HasValue ?
                placeRepository.GetHasNextPageAsync(first, afterCursor, cancellationToken) 
                : Task.FromResult(false);
        }

        private static Task<bool> GetHasPreviousPageAsync(IPlaceRepository placeRepository, int? last,
            DateTime? beforeCursor, CancellationToken cancellationToken)
        {
            return last.HasValue 
                ? placeRepository.GetHasPreviousPageAsync(last, beforeCursor, cancellationToken) 
                : Task.FromResult(false);
        }

        private static Task<List<Tag>> GetTagsAsync(ITagRepository tagRepository, int? first, DateTime? afterCursor,
            int? last, DateTime? beforeCursor, CancellationToken cancellationToken)
        {
            Task<List<Tag>> getTagsTask;
            if (first.HasValue)
            {
                getTagsTask = tagRepository.GetTagsAsync(first, afterCursor, cancellationToken);
            }
            else
            {
                getTagsTask = tagRepository.GetTagsReverseAsync(last, beforeCursor, cancellationToken);
            }

            return getTagsTask;
        }

        private static Task<bool> GetHasNextPageAsync(ITagRepository tagRepository, int? first, DateTime? afterCursor,
            CancellationToken cancellationToken)
        {
            return first.HasValue ?
                tagRepository.GetHasNextPageAsync(first, afterCursor, cancellationToken) 
                : Task.FromResult(false);
        }

        private static Task<bool> GetHasPreviousPageAsync(ITagRepository tagRepository, int? last,
            DateTime? beforeCursor, CancellationToken cancellationToken)
        {
            return last.HasValue 
                ? tagRepository.GetHasPreviousPageAsync(last, beforeCursor, cancellationToken) 
                : Task.FromResult(false);
        }
    }
}