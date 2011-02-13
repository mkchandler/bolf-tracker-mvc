using System.Collections.Generic;

namespace BolfTracker.Repositories
{
    public interface IRepository<TModel>
    {
        void Add(TModel model);

        void Delete(TModel model);

        TModel GetById(int id);

        IEnumerable<TModel> All();
    }
}