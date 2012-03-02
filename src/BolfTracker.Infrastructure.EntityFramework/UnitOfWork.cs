using System.Data.Entity;
namespace BolfTracker.Infrastructure.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private Database _database;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            Check.Argument.IsNotNull(databaseFactory, "databaseFactory");

            _databaseFactory = databaseFactory;
        }

        protected Database Database
        {
            get
            {
                return _database ?? (_database = _databaseFactory.Get());
            }
        }

        public void Commit()
        {
            Database.Commit();
        }
    }

    public class UnitOfWorkDbContext : IUnitOfWork
    {
        private DbContext _context;

        public UnitOfWorkDbContext(DbContext context)
        {
            _context = context;
        }

        protected DbContext Context
        {
            get
            {
                return _context;
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}