using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

using Dapper;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerGameStatisticsRepository : RepositoryBase<PlayerGameStatistics>, IPlayerGameStatisticsRepository
    {
        public PlayerGameStatisticsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public IEnumerable<PlayerGameStatistics> GetByPlayer(int playerId)
        {
            var playerGameStatistics = Database.PlayerGameStatistics.Include(pgs => pgs.Player).Include(pgs => pgs.Game).Where(pgs => pgs.Player.Id == playerId).ToList();

            return playerGameStatistics;
        }

        public IEnumerable<PlayerGameStatistics> GetByMonthAndYear(int month, int year)
        {
            using (var connection = DatabaseFactory.GetProfiledConnection())
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
            var playerGameStatistics = Database.PlayerGameStatistics.Include(pgs => pgs.Player).Include(pgs => pgs.Game).Where(pgs => pgs.Player.Id == playerId && pgs.Game.Date.Month == month && pgs.Game.Date.Year == year).ToList();

            return playerGameStatistics;
        }
    }
}