using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Data.Context;
using GraphQL.Data.Extensions;

namespace GraphQL.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationContext _context;
        
        public IObservable<Account> WhenAccountCreated { get; }
        
        public AccountRepository(ApplicationContext context)
        {
            _context = context;
        }
        
        public async Task<Account> AddAccountAsync(Account account, CancellationToken cancellationToken)
        {
            if (account is null)
            {
                throw new ArgumentNullException(nameof(account));
            }
            
            account.Id = Guid.NewGuid();
            await _context.Set<Account>().AddAsync(account, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            // WhenAccountCreated.OnNext(human);

            return account;
        }

        public Task<List<Account>> GetFriendsAsync(Account account, CancellationToken cancellationToken)
        {
            if (account is null)
            {
                throw new ArgumentNullException(nameof(account));
            }
            
            return Task.FromResult(_context.Set<Account>().Where(x => account.Friends.Any(friend => friend.Id == x.Id)).ToList());
        }

        public Task<List<Tag>> GetSharedTagsAsync(Account account, CancellationToken cancellationToken)
        {
            if (account is null)
            {
                throw new ArgumentNullException(nameof(account));
            }
            
            return Task.FromResult(_context.Set<Tag>()
                .Where(x => account.SharedTags.Any(tag => tag.Id == x.Id)).ToList());
        }

        public Task<List<Place>> GetAppearsInAsync(Account account, CancellationToken cancellationToken)
        {
            if (account is null)
            {
                throw new ArgumentNullException(nameof(account));
            }
            
            return Task.FromResult(_context.Set<Place>()
                .Where(x => account.AppearsIn.Any(place => place.Id == x.Id)).ToList());
        }

        public Task<Account> GetAccountAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Account>().FirstOrDefault(x => x.Id == id));
        }

        public Task<List<Account>> GetAccountsAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Account>()
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter.Value))
                .If(first.HasValue, x => x.Take(first.Value))
                .ToList());
        }

        public Task<List<Account>> GetAccountsReverseAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Account>()
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore.Value))
                .If(last.HasValue, x => x.TakeLast(last.Value))
                .ToList());
        }

        public Task<bool> GetHasNextPageAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Account>()
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter.Value))
                .Skip(first.Value).Any());
        }

        public Task<bool> GetHasPreviousPageAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Account>()
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore.Value))
                .SkipLast(last.Value).Any());
        }

        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Account>().Count());
        }
    }
}