namespace BolfTracker.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}