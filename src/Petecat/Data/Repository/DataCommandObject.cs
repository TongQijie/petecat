using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Petecat.Data.Repository
{
    public class DataCommandObject : IDataCommand
    {
        private DbProviderFactory _DbProviderFactory = null;

        private DatabaseObject _DatabaseObject = null;

        private DbCommand _DbCommand = null;

        public DataCommandObject(DbProviderFactory dbProviderFactory, CommandType commandType, string commandText)
        {
            if (dbProviderFactory == null)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, "dbProviderFactory is null.");
                throw new ArgumentNullException("dbProviderFactory");
            }

            _DbProviderFactory = dbProviderFactory;
            _DbCommand = dbProviderFactory.CreateCommand();
            _DbCommand.CommandText = commandText.Replace('．', '.').Replace('　', ' ');
            _DbCommand.CommandType = commandType;
        }

        public DataCommandObject(DatabaseObject databaseObject, CommandType commandType, string commandText)
        {
            if (databaseObject == null)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, "databaseObject is null.");
                throw new ArgumentNullException("databaseObject");
            }

            _DatabaseObject = databaseObject;
            _DbProviderFactory = databaseObject.DbProviderFactory;
            _DbCommand = _DbProviderFactory.CreateCommand();
            _DbCommand.CommandText = commandText.Replace('．', '.').Replace('　', ' ');
            _DbCommand.CommandType = commandType;
        }

        public DbCommand GetDbCommand()
        {
            return _DbCommand;
        }

        public object GetParameterValue(string parameterName)
        {
            if (_DbCommand.Parameters.Contains(parameterName))
            {
                return _DbCommand.Parameters[parameterName].Value;
            }
            else
            {
                return null;
            }
        }

        public void AddParameter(string parameterName, DbType dbType, ParameterDirection direction, int size)
        {
            if (!_DbCommand.Parameters.Contains(parameterName))
            {
                var dbParameter = _DbProviderFactory.CreateParameter();
                dbParameter.ParameterName = parameterName;
                dbParameter.DbType = dbType;
                dbParameter.Direction = direction;
                dbParameter.Size = size;
                _DbCommand.Parameters.Add(dbParameter);
            }
        }

        public void SetParameterValue(string parameterName, object parameterValue)
        {
            if (!_DbCommand.Parameters.Contains(parameterName))
            {
                throw new ArgumentException("parameter does not exists.");
            }
            else
            {
                _DbCommand.Parameters[parameterName].Value = parameterValue;
            }
        }

        public T QueryScalar<T>()
        {
            if (_DatabaseObject == null)
            {
                throw new NullReferenceException("DatabaseObject is null");
            }

            using (var db = _DatabaseObject)
            {
                return db.QueryScalar<T>(this);
            }
        }

        public List<T> QueryEntities<T>() where T : class, new()
        {
            if (_DatabaseObject == null)
            {
                throw new NullReferenceException("DatabaseObject is null");
            }

            using (var db = _DatabaseObject)
            {
                return db.QueryEntities<T>(this);
            }
        }

        public int ExecuteNonQuery()
        {
            if (_DatabaseObject == null)
            {
                throw new NullReferenceException("DatabaseObject is null");
            }

            using (var db = _DatabaseObject)
            {
                return db.ExecuteNonQuery(this);
            }
        }
    }
}
