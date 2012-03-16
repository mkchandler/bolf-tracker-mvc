using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerStatisticsRepository : IPlayerStatisticsRepository
    {
        public PlayerStatistics GetById(int id)
        {
            using (var context = new BolfTrackerContext())
            {
                var playerStatistic = context.PlayerStatistics.SingleOrDefault(ps => ps.Id == id);

                return playerStatistic;
            }
        }

        public PlayerStatistics GetByPlayerMonthAndYear(int playerId, int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                var playerStatistics = context.PlayerStatistics.Include(ps => ps.Player).SingleOrDefault(ps => ps.Player.Id == playerId && ps.Month == month && ps.Year == year);

                return playerStatistics;
            }
        }

        public IEnumerable<PlayerStatistics> GetByPlayer(int playerId)
        {
            using (var context = new BolfTrackerContext())
            {
                var playerStatistics = context.PlayerStatistics.Include(ps => ps.Player).Where(ps => ps.Player.Id == playerId).ToList();

                return playerStatistics;
            }
        }

        public IEnumerable<PlayerStatistics> GetByMonthAndYear(int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                var playerStatistics = context.PlayerStatistics.Include(ps => ps.Player).Where(ps => ps.Month == month && ps.Year == year).ToList();

                return playerStatistics;
            }
        }

        public IEnumerable<PlayerStatistics> All()
        {
            using (var context = new BolfTrackerContext())
            {
                return context.PlayerStatistics.ToList();
            }
        }

        public void Add(PlayerStatistics model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.PlayerStatistics.Attach(model);
                context.Entry(model).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(PlayerStatistics model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.PlayerStatistics.Remove(model);
                context.SaveChanges();
            }
        }

        public void DeleteByMonthAndYear(int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Database.ExecuteSqlCommand("DELETE FROM PlayerStatistics WHERE Month = @Month AND Year = @Year", new SqlParameter { ParameterName = "Month", Value = month }, new SqlParameter { ParameterName = "Year", Value = year });
            }
        }
    }
}
