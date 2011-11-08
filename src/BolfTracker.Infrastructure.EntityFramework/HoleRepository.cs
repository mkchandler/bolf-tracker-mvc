using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class HoleRepository : RepositoryBase<Hole>, IHoleRepository
    {
        public HoleRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public IEnumerable<Hole> GetActiveByMonthAndYear(int month, int year)
        {
            var holes = Database.Shots.Where(shot => shot.Game.Date.Month == month && shot.Game.Date.Year == year).Select(shot => shot.Hole).Include(hole => hole.Shots).Distinct().ToList();
           
            return holes;
        }
    }
}