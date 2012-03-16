using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IHoleStatisticsRepository : IRepository<HoleStatistics>
    {
        IEnumerable<HoleStatistics> GetByMonthAndYear(int month, int year);

        void DeleteByMonthAndYear(int month, int year);
    }
}
