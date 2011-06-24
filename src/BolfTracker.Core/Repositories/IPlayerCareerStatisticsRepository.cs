using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IPlayerCareerStatisticsRepository : IRepository<PlayerCareerStatistics>
    {
        PlayerCareerStatistics GetByPlayer(int playerId);
    }
}