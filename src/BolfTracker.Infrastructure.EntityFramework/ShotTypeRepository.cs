using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class ShotTypeRepository : IShotTypeRepository
    {
        public ShotType GetById(int id)
        {
            using (var context = new BolfTrackerContext())
            {
                var shotType = context.ShotTypes.SingleOrDefault(st => st.Id == id);

                return shotType;
            }
        }

        public IEnumerable<ShotType> All()
        {
            using (var context = new BolfTrackerContext())
            {
                return context.ShotTypes.ToList();
            }
        }

        public void Add(ShotType model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.ShotTypes.Attach(model);
                context.Entry(model).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Delete(ShotType model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.ShotTypes.Remove(model);
                context.SaveChanges();
            }
        }
    }
}
