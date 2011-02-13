namespace BolfTracker.Infrastructure.EntityFramework.Query
{
    public abstract class QueryBase<TResult> : IQuery<TResult>
    {
        protected QueryBase(bool useCompiled)
        {
            UseCompiled = useCompiled;
        }

        protected bool UseCompiled
        {
            get;
            private set;
        }

        public abstract TResult Execute(Database database);
    }
}