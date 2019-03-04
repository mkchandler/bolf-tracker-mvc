﻿using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

using Dapper;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class HoleStatisticsRepository : IHoleStatisticsRepository
    {
        public HoleStatistics GetById(int id)
        {
            using (var context = new BolfTrackerContext())
            {
                var holeStatistic = context.HoleStatistics.SingleOrDefault(hs => hs.Id == id);

                return holeStatistic;
            }
        }

        public IEnumerable<HoleStatistics> All()
        {
            using (var connection = BolfTrackerDbConnection.GetConnection())
            {
                connection.Open();

                string query =
                    "SELECT SUM(hs.ShotsMade) AS ShotsMade, " +
                    "SUM(hs.Attempts) AS Attempts, " +
                    "SUM(CONVERT(decimal,hs.ShotsMade)) / SUM(CONVERT(decimal,hs.Attempts)) AS ShootingPercentage, " +
                    "SUM(hs.PointsScored) AS PointsScored, SUM(hs.Pushes) AS Pushes, SUM(hs.Steals) AS Steals, " +
                    "SUM(hs.SugarFreeSteals) AS SugarFreeSteals " +
                    "FROM HoleStatistics hs INNER JOIN Hole h ON h.Id = hs.HoleId GROUP BY h.Id HAVING SUM(Attempts) > 0 ";

                var holeStatistics = connection.Query<HoleStatistics>(query).ToList();

                return holeStatistics;
            }
        }

        public IEnumerable<HoleStatistics> GetByMonthAndYear(int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                var holeStatistics = context.HoleStatistics.Include(hs => hs.Hole).Where(hs => hs.Month == month && hs.Year == year).ToList();

                return holeStatistics;
            }
        }

        public void DeleteByMonthAndYear(int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Database.ExecuteSqlCommand("DELETE FROM HoleStatistics WHERE Month = @Month AND Year = @Year", new SqlParameter { ParameterName = "Month", Value = month }, new SqlParameter { ParameterName = "Year", Value = year });
            }
        }

        public void Add(HoleStatistics model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.HoleStatistics.Attach(model);
                context.Entry(model).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(HoleStatistics model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.HoleStatistics.Remove(model);
                context.SaveChanges();
            }
        }
    }
}
