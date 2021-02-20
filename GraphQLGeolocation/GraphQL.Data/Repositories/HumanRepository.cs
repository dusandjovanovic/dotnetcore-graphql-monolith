using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Data.InMemory;

namespace GraphQL.Data.Repositories
{
    public class HumanRepository : IHumanRepository, IDisposable
    {
        public HumanRepository()
        {
            // whenHumanCreated = new Subject<Human>();
        }

        public Task<Human> AddHumanAsync(Human human, CancellationToken cancellationToken)
        {
            if (human is null)
            {
                throw new ArgumentNullException(nameof(human));
            }
            
            human.Id = Guid.NewGuid();
            Database.Humans.Add(human);
            // whenHumanCreated.OnNext(human);

            return Task.FromResult(human);
        }

        public Task<List<Character>> GetFriendsAsync(Human human, CancellationToken cancellationToken)
        {
            if (human is null)
            {
                throw new ArgumentNullException(nameof(human));
            }
            
            return Task.FromResult(Database.Characters
                .Where(x => human.Friends.Contains(x.Id)).ToList());
        }

        public Task<Human> GetHumanAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(Database.Humans.FirstOrDefault(x => x.Id == id));
        }

        public void Dispose()
        {
            // whenHumanCreated.Dispose();
        }
        
        // private readonly Subject<Human> whenHumanCreated;
        
        public IObservable<Human> WhenHumanCreated { get; }
    }
}