using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IRankingRepository : IRepository<Ranking>
    {
        IEnumerable<Ranking> GetByMonthAndYear(int month, int year);
    }
}