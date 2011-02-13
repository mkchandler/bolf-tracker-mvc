using System;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public interface IDatabaseFactory : IDisposable
    {
        Database Get();
    }
}