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
    public class RankingRepository : RepositoryBase<Ranking>, IRankingRepository
    {
        public RankingRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public int GetEligibilityLine(int month, int year)
        {
            using (var connection = DatabaseFactory.GetProfiledConnection())
            {
                connection.Open();

                return connection.Query<int>("GetEligibilityLine", new { Month = month, Year = year }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public IEnumerable<Ranking> GetByMonthAndYear(int month, int year)
        {
            var rankings = Database.Rankings.Include(ranking => ranking.Player).Where(ranking => ranking.Month == month && ranking.Year == year).ToList();

            return rankings;
        }

        public void DeleteByMonthAndYear(int month, int year)
        {
            Database.ExecuteStoreCommand("DELETE FROM Ranking WHERE Month = @Month AND Year = @Year", new SqlParameter { ParameterName = "Month", Value = month }, new SqlParameter { ParameterName = "Year", Value = year });
        }
    }
}
