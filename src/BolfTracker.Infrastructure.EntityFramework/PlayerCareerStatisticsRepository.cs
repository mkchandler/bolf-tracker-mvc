using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerCareerStatisticsRepository : IPlayerCareerStatisticsRepository
    {
        public PlayerCareerStatistics GetById(int id)
        {
            using (var context = new BolfTrackerContext())
            {
                var query = context.PlayerCareerStatistics.SingleOrDefault(pcs => pcs.Id == id);

                return query;
            }
        }

        public PlayerCareerStatistics GetByPlayer(int playerId)
        {
            using (var context = new BolfTrackerContext())
            {
                var query = context.PlayerCareerStatistics.Include(pcs => pcs.Player).FirstOrDefault(pcs => pcs.Player.Id == playerId);

                return query;
            }
        }

        public IEnumerable<PlayerCareerStatistics> All()
        {
            using (var context = new BolfTrackerContext())
            {
                return context.PlayerCareerStatistics.Include(pcs => pcs.Player).ToList();
            }
        }

        public void Add(PlayerCareerStatistics model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.PlayerCareerStatistics.Add(model);

                // Don't add any of the supporting data, it already exists in the database
                context.Entry(model.Player).State = EntityState.Unchanged;

                context.SaveChanges();
            }
        }

        public void Delete(PlayerCareerStatistics model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.PlayerCareerStatistics.Remove(model);
                context.SaveChanges();
            }
        }

        public void DeleteAll()
        {
            using (var context = new BolfTrackerContext())
            {
                context.Database.ExecuteSqlCommand("DELETE FROM PlayerCareerStatistics");
            }
        }
    }
}
