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
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationContext _context;
        
        public IObservable<Tag> WhenTagCreated { get; }
        
        public TagRepository(ApplicationContext context)
        {
            _context = context;
        }
        
        public async Task<Tag> AddTagAsync(Tag tag, CancellationToken cancellationToken)
        {
            if (tag is null)
            {
                throw new ArgumentNullException(nameof(tag));
            }
            
            tag.Id = Guid.NewGuid();
            await _context.Set<Tag>().AddAsync(tag, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            // WhenTagCreated.OnNext(human);

            return tag;
        }

        public Task<Tag> GetTagAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Tag>().FirstOrDefault(x => x.Id == id));
        }

        public Task<List<Tag>> GetTagsAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Tag>()
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter.Value))
                .If(first.HasValue, x => x.Take(first.Value))
                .ToList());
        }

        public Task<List<Tag>> GetTagsReverseAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Tag>()
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore.Value))
                .If(last.HasValue, x => x.TakeLast(last.Value))
                .ToList());
        }

        public Task<bool> GetHasNextPageAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Tag>()
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter.Value))
                .Skip(first.Value).Any());
        }

        public Task<bool> GetHasPreviousPageAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Tag>()
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore.Value))
                .SkipLast(last.Value).Any());
        }

        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Tag>().Count());
        }
    }
}