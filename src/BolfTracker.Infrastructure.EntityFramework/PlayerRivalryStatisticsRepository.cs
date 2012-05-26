using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BolfTracker.Models;
using BolfTracker.Repositories;

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
    }
}
