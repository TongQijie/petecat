using System;
using System.Data;
using System.Text;
using System.Data.Common;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Petecat.EntityFramework
{
    public class DatabaseCommand : IDatabaseCommand
    {
        private DbProviderFactory _DbProviderFactory = null;

        private DbCommand _DbCommand = null;

        public IDatabase Database { get; private set; }

        public DatabaseCommand(DbProviderFactory dbProviderFactory, CommandType commandType, string commandText)
        {
            _DbProviderFactory = dbProviderFactory;
            _DbCommand = dbProviderFactory.CreateCommand();
            _DbCommand.CommandText = commandText.Replace('．', '.').Replace('　', ' ');
            _DbCommand.CommandType = commandType;
        }

        public DatabaseCommand(IDatabase database, CommandType commandType, string commandText)
            : this(database.DbProviderFactory, commandType, commandText)
        {
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

        public T GetScalar<T>()
        {
            using (Database)
            {
                return Database.GetScalar<T>(this);
            }
        }

        public List<T> GetEntities<T>() where T : class
        {
            using (Database)
            {
                return Database.GetEntities<T>(this);
            }
        }

        public int ExecuteNonQuery()
        {
            using (Database)
            {
                return Database.ExecuteNonQuery(this);
            }
        }
    }
}
