using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Data.Repositories
{
    public class SkaterStatisticRepository : ISkaterStatisticRepository
    {
        private readonly StatsContext _dbContext;

        public SkaterStatisticRepository(StatsContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<List<SkaterStatistic>> Get(int playerId)
        {
            return await _dbContext.SkaterStatistics
                .Include(ss => ss.Season)
                .Include(ss => ss.Team)
                .Where(ss => ss.PlayerId == playerId)
                .ToListAsync();
        }
    }
}