using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IHoleRepository : IRepository<Hole>
    {
        IEnumerable<Hole> GetActiveByMonthAndYear(int month, int year);
    }
}
