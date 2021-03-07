using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Core.Models;

namespace GraphQL.Core.Data
{
    public interface IPlaceRepository
    {
        IObservable<Place> WhenPlaceCreated { get; }

        Task<Place> AddPlaceAsync(Place place, CancellationToken cancellationToken);

        Task<List<Place>> GetTagsAsync(Place place, CancellationToken cancellationToken);

        Task<Place> GetPlaceAsync(Guid id, CancellationToken cancellationToken);

        Task<List<Place>> GetPlacesAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken);
        
        Task<List<Place>> GetPlacesReverseAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken);

        Task<bool> GetHasNextPageAsync(int? first, DateTime? createdAfter, CancellationToken cancellationToken);
        
        Task<bool> GetHasPreviousPageAsync(int? last, DateTime? createdBefore, CancellationToken cancellationToken);

        Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
    }
}