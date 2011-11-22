using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private static readonly object _syncObject = new object();

        private readonly DbProviderFactory _providerFactory;
        private readonly string _connectionString;

        private static DbCompiledModel _model;

        private Database _database;

        public DatabaseFactory(DbProviderFactory providerFactory, string connectionString)
        {
            Check.Argument.IsNotNull(providerFactory, "providerFactory");
            Check.Argument.IsNotNullOrEmpty(connectionString, "connectionString", "Connection string cannot be empty");

            _providerFactory = providerFactory;
            _connectionString = connectionString;
        }

        public Database Get()
        {
            if (_database == null)
            {
                DbConnection connection = _providerFactory.CreateConnection();

                if (connection != null)
                {
                    connection.ConnectionString = _connectionString;

                    if (_model == null)
                    {
                        lock (_syncObject)
                        {
                            if (_model == null)
                            {
                                _model = CreateDbModel(connection);
                            }
                        }
                    }

                    _database = _model.CreateObjectContext<Database>(connection);
                }

                return _database;
            }

            return _database;
        }

        public void Dispose()
        {
            if (_database != null)
            {
                _database.Dispose();
            }
        }

        private static DbCompiledModel CreateDbModel(DbConnection connection)
        {
            var modelBuilder = new DbModelBuilder();

            IEnumerable<Type> configurationTypes = typeof(DatabaseFactory).Assembly
                .GetTypes()
                .Where(
                    type =>
                    type.IsPublic && type.IsClass && !type.IsAbstract && !type.IsGenericType && type.BaseType != null &&
                    type.BaseType.IsGenericType &&
                    (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>) ||
                     type.BaseType.GetGenericTypeDefinition() == typeof(ComplexTypeConfiguration<>)) && (type.GetConstructor(Type.EmptyTypes) != null));

            foreach (var configuration in configurationTypes.Select(Activator.CreateInstance))
            {
                modelBuilder.Configurations.Add((dynamic)configuration);
            }

            return modelBuilder.Build(connection).Compile();
        }
    }
}