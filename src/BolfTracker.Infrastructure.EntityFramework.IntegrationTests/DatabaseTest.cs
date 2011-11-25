using System;
using System.Configuration;
using System.Data.Common;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    public abstract class DatabaseTest : IDisposable
    {
        protected readonly DatabaseFactory DatabaseFactory;
        protected readonly UnitOfWork UnitOfWork;

        protected DatabaseTest()
        {
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["BolfTracker"];

            var providerName = connectionStringSettings.ProviderName;
            var connectionString = connectionStringSettings.ConnectionString;

            var providerFactory = DbProviderFactories.GetFactory(providerName);

            DatabaseFactory = new DatabaseFactory(providerFactory, connectionString);

            UnitOfWork = new UnitOfWork(DatabaseFactory);
        }

        public virtual void Dispose()
        {
            DatabaseFactory.Dispose();
        }
    }
}