using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class ShotTypeRepository : RepositoryBase<ShotType>, IShotTypeRepository
    {
        public ShotTypeRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}