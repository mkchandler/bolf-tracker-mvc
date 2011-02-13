namespace BolfTracker.Infrastructure.EntityFramework
{
    public interface IQuery<TResult>
    {
        TResult Execute(Database database);
    }
}