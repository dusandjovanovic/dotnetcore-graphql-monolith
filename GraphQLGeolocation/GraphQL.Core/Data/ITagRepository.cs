using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Core.Models;

namespace GraphQL.Core.Data
{
    public interface ITagRepository
    {
        IObservable<Tag> WhenTagCreated { get; }

        Task<Tag> AddTagAsync(Tag tag, CancellationToken cancellationToken);

        Task<Tag> GetTagAsync(Guid id, CancellationToken cancellationToken);

        Task<List<Tag>> GetTagsAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken);
        
        Task<List<Tag>> GetTagsReverseAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken);

        Task<bool> GetHasNextPageAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken);
        
        Task<bool> GetHasPreviousPageAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken);

        Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
    }
}