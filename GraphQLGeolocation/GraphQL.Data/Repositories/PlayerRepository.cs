using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Core.Data;
using GraphQL.Core.Models;
using GraphQL.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly StatsContext _dbContext;

        public PlayerRepository(StatsContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Player> Get(int id)
        {
            return await _dbContext.Players.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Player> GetRandom()
        {
            return await _dbContext.Players.OrderBy(o => Guid.NewGuid()).FirstOrDefaultAsync();
        }

        public async Task<List<Player>> All()
        {
            return await _dbContext.Players.ToListAsync();
        }

        public async Task<Player> Add(Player player)
        {
            await _dbContext.Players.AddAsync(player);
            await _dbContext.SaveChangesAsync();
            return player;
        }
    }
}