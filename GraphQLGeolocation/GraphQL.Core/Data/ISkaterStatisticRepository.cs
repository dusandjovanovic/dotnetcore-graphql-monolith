using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Core.Models;

namespace GraphQL.Core.Data
{
    public interface ISkaterStatisticRepository
    {
        Task<List<SkaterStatistic>> Get(int playerId);
    }
}