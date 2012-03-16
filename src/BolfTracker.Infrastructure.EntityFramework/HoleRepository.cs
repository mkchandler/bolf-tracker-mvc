using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class HoleRepository : IHoleRepository
    {
        public Hole GetById(int id)
        {
            using (var context = new BolfTrackerContext())
            {
                var hole = context.Holes.SingleOrDefault(g => g.Id == id);

                return hole;
            }
        }

        public IEnumerable<Hole> GetActiveByMonthAndYear(int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                var holes = context.Shots.Where(shot => shot.Game.Date.Month == month && shot.Game.Date.Year == year).Select(shot => shot.Hole).Include(hole => hole.Shots.Where(shot => shot.Game.Date.Month == month && shot.Game.Date.Year == year)).Distinct().ToList();

                return holes;
            }
        }

        public IEnumerable<Hole> All()
        {
            using (var context = new BolfTrackerContext())
            {
                return context.Holes.ToList();
            }
        }

        public void Add(Hole model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Holes.Attach(model);
                context.Entry(model).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(Hole model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Holes.Remove(model);
                context.SaveChanges();
            }
        }
    }
}
