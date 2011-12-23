using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IGameStatisticsRepository : IRepository<GameStatistics>
    {
        void DeleteAll();

        void DeleteByMonthAndYear(int month, int year);
    }
}
