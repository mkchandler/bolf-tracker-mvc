using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

using Dapper;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class GameRepository : IGameRepository
    {
        public Game GetById(int id)
        {
            using (var context = new BolfTrackerContext())
            {
                var game = context.Games.Include(g => g.GameStatistics).SingleOrDefault(g => g.Id == id);

                return game;
            }
        }

        public IEnumerable<Tuple<int, int>> GetActiveMonthsAndYears()
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();

                string query = "SELECT (DATEPART(MONTH, [Date])) AS Month, (DATEPART(YEAR, [Date])) AS Year FROM Game GROUP BY (DATEPART(MONTH, [Date])), (DATEPART (year, [Date]))";

                var rows = connection.Query(query);
                var monthsAndYears = new List<Tuple<int, int>>();

                foreach (var row in rows)
                {
                    monthsAndYears.Add(new Tuple<int, int>(row.Month, row.Year));
                }

                return monthsAndYears;
            }
        }

        public IEnumerable<Game> GetAllFinalized()
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();

                string query = "SELECT g.* FROM Game g INNER JOIN GameStatistics gs ON gs.GameId = g.Id";

                var games = connection.Query<Game>(query).ToList();

                return games;
            }
        }

        public IEnumerable<Game> GetByMonthAndYear(int month, int year)
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Game WHERE (DATEPART (month, [Date])) = @Month AND (DATEPART (year, [Date])) = @Year";

                var games = connection.Query<Game>(query, new { Month = month, Year = year }).ToList();

                return games;
            }
        }

        public IEnumerable<Game> GetFinalizedByMonthAndYear(int month, int year)
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();

                string query = "SELECT g.* FROM Game g INNER JOIN GameStatistics gs ON gs.GameId = g.Id WHERE (DATEPART (month, g.[Date])) = @Month AND (DATEPART (year, g.[Date])) = @Year";

                var games = connection.Query<Game>(query, new { Month = month, Year = year }).ToList();

                return games;
            }
        }

        public IEnumerable<Game> All()
        {
            using (var context = new BolfTrackerContext())
            {
                return context.Games.ToList();
            }
        }

        public void Add(Game model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Games.Attach(model);
                context.Entry(model).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(Game model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Games.Remove(model);
                context.SaveChanges();
            }
        }
    }
}
