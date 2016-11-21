using Petecat.EntityFramework.Internal;

using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

namespace Petecat.EntityFramework
{
    public class Database : IDatabase
    {
        public DbProviderFactory DbProviderFactory { get; private set; }

        protected string _ConnectionString = null;

        protected DbConnection _DbConnection = null;

        private DbTransaction _DbTransaction = null;

        public Database(DbProviderFactory dbProviderFactory, string connectionString)
        {
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

        private DbCommand CreateDbCommnad(IDatabaseCommand dataCommand)
        {
            var dbCommand = dataCommand.GetDbCommand();
            dbCommand.Connection = OpenConnection();
            dbCommand.Transaction = _DbTransaction;
            return dbCommand;
        }

        public int ExecuteNonQuery(IDatabaseCommand dataCommand)
        {
            return CreateDbCommnad(dataCommand).ExecuteNonQuery();
        }

        public bool ExecuteTransaction(Func<IDatabase, bool> execution)
        {
            var success = false;
            try
            {
                _DbTransaction = OpenConnection().BeginTransaction();
                success = execution(this);
            }
            catch (Exception e)
            {
                throw new Exception("failed to execute transaction.", e);
            }
            finally
            {
                if (_DbTransaction != null)
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
            }

            return success;
        }

        public List<T> GetEntities<T>(IDatabaseCommand dataCommand) where T : class
        {
            var result = new List<T>();
            using (var reader = CreateDbCommnad(dataCommand).ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add((T)EntityBuilder.GetBuilder(typeof(T)).Build(reader));
                }
            }
            return result;
        }

        public T GetScalar<T>(IDatabaseCommand dataCommand)
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
