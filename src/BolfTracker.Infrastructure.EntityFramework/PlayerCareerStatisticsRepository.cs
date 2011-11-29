using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerCareerStatisticsRepository : RepositoryBase<PlayerCareerStatistics>, IPlayerCareerStatisticsRepository
    {
        public PlayerCareerStatisticsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public PlayerCareerStatistics GetByPlayer(int playerId)
        {
            var query = Database.PlayerCareerStatistics.First(pcs => pcs.Player.Id == playerId);

            return query;
        }
    }
}