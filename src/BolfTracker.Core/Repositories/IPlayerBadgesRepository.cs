using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BolfTracker.Models;

namespace BolfTracker.Repositories
{
   public interface IPlayerBadgesRepository : IRepository<PlayerBadges>
    {
        IEnumerable<PlayerBadges> GetByPlayer(int playerId);
    }
}
