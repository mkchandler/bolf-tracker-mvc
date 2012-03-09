﻿using System.Collections.Generic;
using System.Data;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

using Dapper;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class GameStatisticsRepository : IGameStatisticsRepository
    {
        public GameStatistics GetById(int id)
        {
            using (var context = new BolfTrackerContext())
            {
                var gameStatistic = context.GameStatistics.SingleOrDefault(gs => gs.Id == id);

                return gameStatistic;
            }
        }

        public IEnumerable<GameStatistics> All()
        {
            using (var context = new BolfTrackerContext())
            {
                return context.GameStatistics.ToList();
            }
        }

        public void Add(GameStatistics model)
        {
            using (var context = new BolfTrackerContext())
            {
                // Add the game stats to the database
                context.GameStatistics.Add(model);

                // Don't add any of the supporting data, it already exists in the database
                context.Entry(model.Game).State = EntityState.Unchanged;

                context.SaveChanges();
            }
        }

        public void Delete(GameStatistics model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.GameStatistics.Remove(model);
                context.SaveChanges();
            }
        }

        public void DeleteAll()
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();

                string command = "DELETE FROM GameStatistics";

                connection.Execute(command);
            }
        }

        public void DeleteByMonthAndYear(int month, int year)
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();

                string command = "DELETE GameStatistics FROM GameStatistics gs INNER JOIN Game g ON g.Id = gs.GameId WHERE (DATEPART (month, g.[Date])) = @Month AND (DATEPART (year, g.[Date])) = @Year";

                connection.Execute(command, new { Month = month, Year = year });
            }
        }
    }
}
