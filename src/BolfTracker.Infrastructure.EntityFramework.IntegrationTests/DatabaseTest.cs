using System;
using System.Configuration;
using System.Data.Common;

using BolfTracker.Infrastructure.EntityFramework.Query;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    public abstract class DatabaseTest : IDisposable
    {
        protected readonly DatabaseFactory DatabaseFactory;
        protected readonly QueryFactory QueryFactory;
        protected readonly UnitOfWork UnitOfWork;

        protected DatabaseTest()
        {
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["BolfTracker"];

            var providerName = connectionStringSettings.ProviderName;
            var connectionString = connectionStringSettings.ConnectionString;

            var providerFactory = DbProviderFactories.GetFactory(providerName);

            DatabaseFactory = new DatabaseFactory(providerFactory, connectionString);
            QueryFactory = new QueryFactory(true, true);

            UnitOfWork = new UnitOfWork(DatabaseFactory);
        }

        public virtual void Dispose()
        {
            DatabaseFactory.Dispose();
        }
    }
}