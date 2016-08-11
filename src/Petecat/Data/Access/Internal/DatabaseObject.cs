using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Data;

namespace Petecat.Data.Access
{
    internal class DatabaseObject : IDatabaseObject
    {
        public DbProviderFactory DbProviderFactory { get; private set; }

        protected string _ConnectionString = null;

        protected DbConnection _DbConnection = null;

        private DbTransaction _DbTransaction = null;

        public DatabaseObject(DbProviderFactory dbProviderFactory, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                Logging.LoggerManager.GetLogger().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, "ConnectionString is empty.");
                throw new ArgumentNullException("connectionString");
            }

            if (dbProviderFactory == null)
            {
                Logging.LoggerManager.GetLogger().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, "dbProviderFactory is null.");
                throw new ArgumentNullException("dbProviderFactory");
            }

            DbProviderFactory = dbProviderFactory;
            _ConnectionString = connectionString;
        }

        private DbConnection CreateConnection()
        {
            if (_DbConnection == null)
            {
                _DbConnection = DbProviderFactory.CreateConnection();
                _DbConnection.ConnectionString = _ConnectionString;
            }

            return _DbConnection;
        }

        private DbConnection OpenConnection()
        {
            if (_DbConnection == null)
            {
                CreateConnection();
            }

            if (_DbConnection.State != ConnectionState.Open)
            {
                _DbConnection.Open();
            }

            return _DbConnection;
        }

        private DbCommand CreateDbCommnad(IDataCommandObject dataCommand)
        {
            var dbCommand = dataCommand.GetDbCommand();
            dbCommand.Connection = OpenConnection();
            dbCommand.Transaction = _DbTransaction;
            return dbCommand;
        }

        public int ExecuteNonQuery(IDataCommandObject dataCommand)
        {
            return CreateDbCommnad(dataCommand).ExecuteNonQuery();
        }

        public bool ExecuteTransaction(Func<IDatabaseObject, bool> execution)
        {
            var success = false;
            try
            {
                _DbTransaction = _DbConnection.BeginTransaction();
                success = execution(this);
            }
            catch (Exception e)
            {
                Logging.LoggerManager.GetLogger().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, "failed to execute transaction.", e);
            }
            finally
            {
                if (success)
                {
                    _DbTransaction.Commit();
                }
                else
                {
                    _DbTransaction.Rollback();
                }
                _DbTransaction = null;
            }

            return success;
        }

        public List<T> QueryEntities<T>(IDataCommandObject dataCommand) where T : class, new()
        {
            var result = new List<T>();
            using (var reader = CreateDbCommnad(dataCommand).ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(Entity.EntityBuilder.BuildEntity<T>(reader));
                }
            }
            return result;
        }

        public T QueryScalar<T>(IDataCommandObject dataCommand)
        {
            return (T)CreateDbCommnad(dataCommand).ExecuteScalar();
        }

        public void Dispose()
        {
            if (_DbConnection != null)
            {
                if (_DbConnection.State == ConnectionState.Open)
                {
                    _DbConnection.Close();
                }
                _DbConnection = null;
            }
        }
    }
}