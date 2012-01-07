using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IRankingRepository : IRepository<Ranking>
    {
        int GetEligibilityLine(int month, int year);

        IEnumerable<Ranking> GetByMonthAndYear(int month, int year);

        void DeleteByMonthAndYear(int month, int year);
    }
}
