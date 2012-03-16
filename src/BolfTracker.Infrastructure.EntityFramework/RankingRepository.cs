using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

using Dapper;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class RankingRepository : IRankingRepository
    {
        public Ranking GetById(int id)
        {
            using (var context = new BolfTrackerContext())
            {
                var ranking = context.Rankings.SingleOrDefault(r => r.Id == id);
                
                return ranking;
            }
        }

        public IEnumerable<Ranking> GetByMonthAndYear(int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                var rankings = context.Rankings.Include(ranking => ranking.Player).Where(ranking => ranking.Month == month && ranking.Year == year).ToList();

                return rankings;
            }
        }

        public IEnumerable<Ranking> All()
        {
            using (var context = new BolfTrackerContext())
            {
                var rankings = context.Rankings.ToList();

                return rankings;
            }
        }

        public int GetEligibilityLine(int month, int year)
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();

                return connection.Query<int>("GetEligibilityLine", new { Month = month, Year = year }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public void Add(Ranking model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Rankings.Attach(model);
                context.Entry(model).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(Ranking model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Rankings.Remove(model);
                context.SaveChanges();
            }
        }

        public void DeleteByMonthAndYear(int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Database.ExecuteSqlCommand("DELETE FROM Ranking WHERE Month = @Month AND Year = @Year", new SqlParameter { ParameterName = "Month", Value = month }, new SqlParameter { ParameterName = "Year", Value = year });
            }
        }
    }
}
