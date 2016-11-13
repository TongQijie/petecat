using System;
using System.Data;
using System.Text;
using System.Data.Common;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Petecat.Data.Access
{
    internal class DataCommandObject : IDataCommandObject
    {
        private DbProviderFactory _DbProviderFactory = null;

        private IDatabaseObject _DatabaseObject = null;

        private DbCommand _DbCommand = null;

        public IDatabaseObject DatabaseObject { get { return _DatabaseObject; } }

        public DataCommandObject(DbProviderFactory dbProviderFactory, CommandType commandType, string commandText)
        {
            if (dbProviderFactory == null)
            {
                //Logging.LoggerManager.GetLogger().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, "dbProviderFactory is null.");
                throw new ArgumentNullException("dbProviderFactory");
            }

            _DbProviderFactory = dbProviderFactory;
            _DbCommand = dbProviderFactory.CreateCommand();
            _DbCommand.CommandText = commandText.Replace('．', '.').Replace('　', ' ');
            _DbCommand.CommandType = commandType;
        }

        public DataCommandObject(IDatabaseObject databaseObject, CommandType commandType, string commandText)
        {
            if (databaseObject == null)
            {
                //Logging.LoggerManager.GetLogger().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, "databaseObject is null.");
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
                if (parameterValue == null)
                {
                    _DbCommand.Parameters[parameterName].Value = DBNull.Value;
                }
                else
                {
                    _DbCommand.Parameters[parameterName].Value = parameterValue;
                }
            }
        }

        public void SetParameterValues(string parameterName, params object[] parameterValues)
        {
            if (!_DbCommand.Parameters.Contains(parameterName))
            {
                throw new ArgumentException("parameter does not exists.");
            }

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < parameterValues.Length; i++)
            {
                var name = parameterName + i;
                stringBuilder.AppendFormat("{0},", name);
                AddParameter(name, _DbCommand.Parameters[parameterName].DbType, ParameterDirection.Input, _DbCommand.Parameters[parameterName].Size);
                SetParameterValue(name, parameterValues[i]);
            }
            stringBuilder.ToString().Trim(',');

            _DbCommand.Parameters.Remove(_DbCommand.Parameters[parameterName]);
            _DbCommand.CommandText = _DbCommand.CommandText.Replace(parameterName, stringBuilder.ToString().Trim(','));
        }

        public void FormatCommandText(int index, params object[] args)
        {
            if (args == null || args.Length == 0 || !Regex.IsMatch(_DbCommand.CommandText, "\\x7b" + index + "\\x7d"))
            {
                return;
            }

            var stringArgs = new string[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is string || args[i] is DateTime)
                {
                    stringArgs[i] = string.Format("'{0}'", args[i].ToString());
                }
                else
                {
                    stringArgs[i] = args[i].ToString();
                }
            }

            _DbCommand.CommandText = Regex.Replace(_DbCommand.CommandText, "\\x7b" + index + "\\x7d", string.Join(",", stringArgs));
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
