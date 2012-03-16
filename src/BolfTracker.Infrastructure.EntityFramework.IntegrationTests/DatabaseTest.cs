using System;
using System.Configuration;
using System.Data.Common;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    public abstract class DatabaseTest : IDisposable
    {
        protected DatabaseTest()
        {
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["BolfTracker"];

            var providerName = connectionStringSettings.ProviderName;
            var connectionString = connectionStringSettings.ConnectionString;

            var providerFactory = DbProviderFactories.GetFactory(providerName);
        }

        public virtual void Dispose()
        {
            DatabaseFactory.Dispose();
        }
    }
}
