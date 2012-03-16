using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

using Dapper;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerGameStatisticsRepository : IPlayerGameStatisticsRepository
    {
        public PlayerGameStatistics GetById(int id)
        {
            using (var context = new BolfTrackerContext())
            {
                var playerGameStatistics = context.PlayerGameStatistics.SingleOrDefault(pgs => pgs.Id == id);

                return playerGameStatistics;
            }
        }

        public IEnumerable<PlayerGameStatistics> GetByPlayer(int playerId)
        {
            using (var context = new BolfTrackerContext())
            {
                var playerGameStatistics = context.PlayerGameStatistics.Include(pgs => pgs.Player).Include(pgs => pgs.Game).Where(pgs => pgs.Player.Id == playerId).ToList();

                return playerGameStatistics;
            }
        }

        public IEnumerable<PlayerGameStatistics> GetByMonthAndYear(int month, int year)
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();

                string query = "SELECT * FROM PlayerGameStatistics pgs " +
                    "INNER JOIN Game g on g.Id = pgs.GameId " +
                    "INNER JOIN Player p ON p.Id = pgs.PlayerId " +
                    "WHERE (DATEPART (month, g.[Date])) = @Month AND (DATEPART (year, g.[Date])) = @Year";

                var playerGameStatistics = connection.Query<PlayerGameStatistics, Game, Player, PlayerGameStatistics>(query, (pgs, g, p) => { pgs.Game = g; pgs.Player = p; return pgs; }, new { Month = month, Year = year }).ToList();

                return playerGameStatistics;
            }
        }

        public IEnumerable<PlayerGameStatistics> GetByPlayerMonthAndYear(int playerId, int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                var playerGameStatistics = context.PlayerGameStatistics.Include(pgs => pgs.Player).Include(pgs => pgs.Game).Where(pgs => pgs.Player.Id == playerId && pgs.Game.Date.Month == month && pgs.Game.Date.Year == year).ToList();

                return playerGameStatistics;
            }
        }

        public IEnumerable<PlayerGameStatistics> All()
        {
            using (var context = new BolfTrackerContext())
            {
                var playerGameStatistics = context.PlayerGameStatistics.ToList();

                return playerGameStatistics;
            }
        }

        public void Add(PlayerGameStatistics model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.PlayerGameStatistics.Attach(model);
                context.Entry(model).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(PlayerGameStatistics model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.PlayerGameStatistics.Remove(model);
                context.SaveChanges();
            }
        }

        public void DeleteAll()
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();

                string command = "DELETE FROM PlayerGameStatistics";

                connection.Execute(command);
            }
        }

        public void DeleteByMonthAndYear(int month, int year)
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();

                string command = "DELETE PlayerGameStatistics FROM PlayerGameStatistics pgs INNER JOIN Game g ON g.Id = pgs.GameId WHERE (DATEPART (month, g.[Date])) = @Month AND (DATEPART (year, g.[Date])) = @Year";

                connection.Execute(command, new { Month = month, Year = year });
            }
        }
    }
}
