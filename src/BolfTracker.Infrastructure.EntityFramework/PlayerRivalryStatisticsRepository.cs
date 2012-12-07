using System;
using System.Collections.Generic;
using System.Data;

using BolfTracker.Models;
using BolfTracker.Repositories;

using Dapper;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerRivalryStatisticsRepository : IPlayerRivalryStatisticsRepository
    {
        public IEnumerable<PlayerRivalryStatistics> GetByPlayer(int playerId)
        {
            throw new NotImplementedException();
        }

        public void Add(PlayerRivalryStatistics model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.PlayerRivalryStatistics.Attach(model);
                context.Entry(model).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(PlayerRivalryStatistics model)
        {
            throw new NotImplementedException();
        }

        public PlayerRivalryStatistics GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PlayerRivalryStatistics> All()
        {
            throw new NotImplementedException();
        }

        public void DeleteAll()
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();
                string command = "DELETE FROM PlayerRivalryStatistics";
                connection.Execute(command);
            }
        }

        public void DeleteByMonthAndYear(int month, int year)
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();
                string command = "DELETE PlayerRivalryStatistics FROM PlayerRivalryStatistics prs INNER JOIN Game g ON g.Id = prs.GameId WHERE (DATEPART (month, g.[Date])) = @Month AND (DATEPART (year, g.[Date])) = @Year";
                connection.Execute(command, new { Month = month, Year = year });
            }
        }

        public void DeleteByGame(int gameId)
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();
                string command = "DELETE FROM PlayerRivalryStatistics WHERE GameId = @GameId";
                connection.Execute(command, new { GameId = gameId });
            }
        }
    }
}
