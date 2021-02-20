using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Core.Models;

namespace GraphQL.Core.Data
{
    public interface IHumanRepository
    {
        IObservable<Human> WhenHumanCreated { get; }

        Task<Human> AddHumanAsync(Human human, CancellationToken cancellationToken);

        Task<List<Character>> GetFriendsAsync(Human human, CancellationToken cancellationToken);

        Task<Human> GetHumanAsync(Guid id, CancellationToken cancellationToken);
    }
}