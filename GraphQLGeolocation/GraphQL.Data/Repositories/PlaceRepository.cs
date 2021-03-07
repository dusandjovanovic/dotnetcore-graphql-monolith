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
    public class PlaceRepository : IPlaceRepository
    {
        public IObservable<Place> WhenPlaceCreated { get; }
        
        public Task<Place> AddPlaceAsync(Place place, CancellationToken cancellationToken)
        {
            if (place is null)
            {
                throw new ArgumentNullException(nameof(place));
            }
            
            place.Id = Guid.NewGuid();
            Database.Places.Add(place);
            // whenHumanCreated.OnNext(human);

            return Task.FromResult(place);
        }

        public Task<List<Place>> GetTagsAsync(Place place, CancellationToken cancellationToken)
        {
            if (place is null)
            {
                throw new ArgumentNullException(nameof(place));
            }

            return Task.FromResult(Database.Places
                .Where(x => place.Tags.Contains(x.Id)).ToList());
        }

        public Task<Place> GetPlaceAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Places.FirstOrDefault(x => x.Id == id));
        }

        public Task<List<Place>> GetPlacesAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Places
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter.Value))
                .If(first.HasValue, x => x.Take(first.Value))
                .ToList());
        }

        public Task<List<Place>> GetPlacesReverseAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Places
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore.Value))
                .If(last.HasValue, x => x.TakeLast(last.Value))
                .ToList());
        }

        public Task<bool> GetHasNextPageAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Places
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter.Value))
                .Skip(first.Value).Any());
        }

        public Task<bool> GetHasPreviousPageAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Places
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore.Value))
                .SkipLast(last.Value).Any());
        }

        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Places.Count);
        }
    }
}