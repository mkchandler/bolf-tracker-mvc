using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IPlayerRivalryStatisticsRepository : IRepository<PlayerRivalryStatistics>
    {
        IEnumerable<PlayerRivalryStatistics> GetByPlayer(int playerId);
    }
}
