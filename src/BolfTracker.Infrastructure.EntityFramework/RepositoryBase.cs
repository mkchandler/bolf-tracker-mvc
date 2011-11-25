using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public abstract class RepositoryBase<TEntity> where TEntity : class, IEntity
    {
        private Database _database;

        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            Check.Argument.IsNotNull(databaseFactory, "databaseFactory");
            //Check.Argument.IsNotNull(queryFactory, "queryFactory");

            DatabaseFactory = databaseFactory;
            //QueryFactory = queryFactory;
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        //protected IQueryFactory QueryFactory
        //{
        //    get;
        //    private set;
        //}

        protected Database Database
        {
            get
            {
                return _database ?? (_database = DatabaseFactory.Get());
            }
        }

        public virtual void Add(TEntity entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Database.ObjectSet<TEntity>().AddObject(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            Check.Argument.IsNotNull(entity, "entity");

            Database.ObjectSet<TEntity>().DeleteObject(entity);
        }

        public virtual TEntity GetById(int id)
        {
            return Database.ObjectSet<TEntity>().SingleOrDefault(entity => entity.Id == id);
        }

        public virtual IEnumerable<TEntity> All()
        {
            return Database.ObjectSet<TEntity>();
        }
    }
}