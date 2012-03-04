using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerHoleStatisticsRepository : IPlayerHoleStatisticsRepository
    {
        public PlayerHoleStatistics GetById(int id)
        {
            using (var context = new BolfTrackerContext())
            {
                var playerHoleStatistics = context.PlayerHoleStatistics.SingleOrDefault(phs => phs.Id == id);

                return playerHoleStatistics;
            }
        }

        public PlayerHoleStatistics GetByPlayerHoleMonthAndYear(int playerId, int holeId, int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                var playerHoleStatistics = context.PlayerHoleStatistics.Include(phs => phs.Player).Include(phs => phs.Hole).First(phs => phs.Player.Id == playerId && phs.Hole.Id == holeId && phs.Month == month && phs.Year == year);

                return playerHoleStatistics;
            }
        }

        public IEnumerable<PlayerHoleStatistics> GetByPlayerMonthAndYear(int playerId, int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                var playerHoleStatistics = context.PlayerHoleStatistics.Include(phs => phs.Player).Include(phs => phs.Hole).Where(phs => phs.Player.Id == playerId && phs.Month == month && phs.Year == year).ToList();

                return playerHoleStatistics;
            }
        }

        public IEnumerable<PlayerHoleStatistics> GetByMonthAndYear(int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                var playerHoleStatistics = context.PlayerHoleStatistics.Include(phs => phs.Player).Include(phs => phs.Hole).Where(phs => phs.Month == month && phs.Year == year).ToList();

                return playerHoleStatistics;
            }
        }

        public IEnumerable<PlayerHoleStatistics> All()
        {
            using (var context = new BolfTrackerContext())
            {
                return context.PlayerHoleStatistics.ToList();
            }
        }

        public void Add(PlayerHoleStatistics model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.PlayerHoleStatistics.Add(model);
                context.SaveChanges();
            }
        }

        public void Delete(PlayerHoleStatistics model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.PlayerHoleStatistics.Remove(model);
                context.SaveChanges();
            }
        }      
  
        public void DeleteByMonthAndYear(int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Database.ExecuteSqlCommand("DELETE FROM PlayerHoleStatistics WHERE Month = @Month AND Year = @Year", new SqlParameter { ParameterName = "Month", Value = month }, new SqlParameter { ParameterName = "Year", Value = year });
            }
        }
    }
}
