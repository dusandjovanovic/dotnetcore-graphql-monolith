using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Data.Extensions;
using GraphQL.Data.InMemory;

namespace GraphQL.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public IObservable<Account> WhenAccountCreated { get; }
        
        public Task<Account> AddAccountAsync(Account account, CancellationToken cancellationToken)
        {
            if (account is null)
            {
                throw new ArgumentNullException(nameof(account));
            }
            
            account.Id = Guid.NewGuid();
            Database.Accounts.Add(account);
            // WhenAccountCreated.OnNext(human);

            return Task.FromResult(account);
        }

        public Task<List<Account>> GetFriendsAsync(Account account, CancellationToken cancellationToken)
        {
            if (account is null)
            {
                throw new ArgumentNullException(nameof(account));
            }
            
            return Task.FromResult(Database.Accounts
                .Where(x => account.Friends.Contains(x.Id)).ToList());
        }

        public Task<List<Tag>> GetSharedTagsAsync(Account account, CancellationToken cancellationToken)
        {
            if (account is null)
            {
                throw new ArgumentNullException(nameof(account));
            }
            
            return Task.FromResult(Database.Tags
                .Where(x => account.SharedTags.Contains(x.Id)).ToList());
        }

        public Task<List<Place>> GetAppearsInAsync(Account account, CancellationToken cancellationToken)
        {
            if (account is null)
            {
                throw new ArgumentNullException(nameof(account));
            }
            
            return Task.FromResult(Database.Places
                .Where(x => account.AppearsIn.Contains(x.Id)).ToList());
        }

        public Task<Account> GetAccountAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Accounts.FirstOrDefault(x => x.Id == id));
        }

        public Task<List<Account>> GetAccountsAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Accounts
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter.Value))
                .If(first.HasValue, x => x.Take(first.Value))
                .ToList());
        }

        public Task<List<Account>> GetAccountsReverseAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Accounts
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore.Value))
                .If(last.HasValue, x => x.TakeLast(last.Value))
                .ToList());
        }

        public Task<bool> GetHasNextPageAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Accounts
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter.Value))
                .Skip(first.Value).Any());
        }

        public Task<bool> GetHasPreviousPageAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Accounts
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore.Value))
                .SkipLast(last.Value).Any());
        }

        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Accounts.Count);
        }
    }
}