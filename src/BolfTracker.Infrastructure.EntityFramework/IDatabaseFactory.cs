using System;
using System.Data.Common;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public interface IDatabaseFactory : IDisposable
    {
        Database Get();

        DbConnection GetProfiledConnection();
    }
}