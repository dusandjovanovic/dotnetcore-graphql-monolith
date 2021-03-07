using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Core.Models;

namespace GraphQL.Core.Data
{
    public interface IAccountRepository
    {
        IObservable<Account> WhenAccountCreated { get; }

        Task<Account> AddAccountAsync(Account account, CancellationToken cancellationToken);

        Task<List<Account>> GetFriendsAsync(Account account, CancellationToken cancellationToken);

        Task<Account> GetAccountAsync(Guid id, CancellationToken cancellationToken);

        Task<List<Account>> GetAccountsAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken);
        
        Task<List<Account>> GetAccountsReverseAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken);

        Task<bool> GetHasNextPageAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken);
        
        Task<bool> GetHasPreviousPageAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken);

        Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
    }
}