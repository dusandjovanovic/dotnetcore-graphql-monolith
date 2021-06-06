using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Core.Models;
using GraphQL.Data.Context;

namespace GraphQL.Data.Repositories
{
    public class AccountRepository : GenericRepository<Account>
    {
        public AccountRepository(ApplicationContext context) : base(context) { }

        public Task<IEnumerable<Account>> GetFriends(int id)
        {
            return Task.FromResult(_entities.Where(e => e.Friends.Any(friend => friend.Id == id)).AsEnumerable());
        }
    }
}