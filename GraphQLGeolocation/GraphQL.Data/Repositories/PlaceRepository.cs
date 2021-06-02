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
    public class PlaceRepository : IPlaceRepository
    {
        private readonly ApplicationContext _context;
        
        public IObservable<Place> WhenPlaceCreated { get; }
        
        public PlaceRepository(ApplicationContext context)
        {
            _context = context;
        }
        
        public async Task<Place> AddPlaceAsync(Place place, CancellationToken cancellationToken)
        {
            if (place is null)
            {
                throw new ArgumentNullException(nameof(place));
            }
            
            place.Id = Guid.NewGuid();
            await _context.Set<Place>().AddAsync(place, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            // WhenPlaceCreated.OnNext(human);

            return place;
        }

        public Task<List<Tag>> GetTagsAsync(Place place, CancellationToken cancellationToken)
        {
            if (place is null)
            {
                throw new ArgumentNullException(nameof(place));
            }
            
            return Task.FromResult(_context.Set<Tag>().Where(x =>  place.Tags.Any(tag => tag.Id == x.Id)).ToList());
        }

        public Task<Place> GetPlaceAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Place>().FirstOrDefault(x => x.Id == id));
        }

        public Task<List<Place>> GetPlacesAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Place>()
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter.Value))
                .If(first.HasValue, x => x.Take(first.Value))
                .ToList());
        }

        public Task<List<Place>> GetPlacesReverseAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Place>()
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore.Value))
                .If(last.HasValue, x => x.TakeLast(last.Value))
                .ToList());
        }

        public Task<bool> GetHasNextPageAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Place>()
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter.Value))
                .Skip(first.Value).Any());
        }

        public Task<bool> GetHasPreviousPageAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Place>()
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore.Value))
                .SkipLast(last.Value).Any());
        }

        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Set<Place>().Count());
        }
    }
}