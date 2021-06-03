using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Core.Models;
using GraphQL.Data.Context;

namespace GraphQL.Data.Repositories
{
    public class ReviewRepository : GenericRepository<Review>
    {
        public ReviewRepository(ApplicationContext context) : base(context) { }

        public Task<IEnumerable<Review>> GetFromAccount(int id)
        {
            return Task.FromResult(_entities.Where(e => e.AccountId == id).AsEnumerable());
        }
    }
}