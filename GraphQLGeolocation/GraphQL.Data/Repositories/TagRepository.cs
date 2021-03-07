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
    public class TagRepository : ITagRepository
    {
        public IObservable<Tag> WhenTagCreated { get; }
        
        public Task<Tag> AddTagAsync(Tag tag, CancellationToken cancellationToken)
        {
            if (tag is null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            
            tag.Id = Guid.NewGuid();
            Database.Tags.Add(tag);
            // WhenTagCreated.OnNext(human);

            return Task.FromResult(tag);
        }

        public Task<Tag> GetTagAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Tags.FirstOrDefault(x => x.Id == id));
        }

        public Task<List<Tag>> GetTagsAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Tags
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter.Value))
                .If(first.HasValue, x => x.Take(first.Value))
                .ToList());
        }

        public Task<List<Tag>> GetTagsReverseAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Tags
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore.Value))
                .If(last.HasValue, x => x.TakeLast(last.Value))
                .ToList());
        }

        public Task<bool> GetHasNextPageAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Tags
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter.Value))
                .Skip(first.Value).Any());
        }

        public Task<bool> GetHasPreviousPageAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Tags
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore.Value))
                .SkipLast(last.Value).Any());
        }

        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Tags.Count);
        }
    }
}