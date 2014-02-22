using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;
namespace BolfTracker.Infrastructure.EntityFramework
{
   public class PlayerBadgesRepository : IPlayerBadgesRepository
    {

       public IEnumerable<PlayerBadges> GetByPlayer(int id)
       {
           using (var context = new BolfTrackerContext())
           {
               var query = context.PlayerBadges.Include(pcs => pcs.Player).Include(p=>p.Badge).Where(pcs => pcs.Player.Id == id);

               return query.ToList();
           }
       }

       public void Add(PlayerBadges model)
       {
           throw new System.NotImplementedException();
       }

       public void Delete(PlayerBadges model)
       {
           throw new System.NotImplementedException();
       }

       public PlayerBadges GetById(int id)
       {
           throw new System.NotImplementedException();
       }

       public IEnumerable<PlayerBadges> All()
       {
           throw new System.NotImplementedException();
       }
    }
}
