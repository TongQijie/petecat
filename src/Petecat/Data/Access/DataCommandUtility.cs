﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

using Petecat.Utility;

namespace Petecat.Data.Access
{
    public static class DataCommandUtility
    {
        /// <summary>
        /// 从数据命令缓存中获取
        /// </summary>
        public static IDataCommand GetDataCommand(string dataCommandName)
        {
            var dataCommand = DataCommandCache.Manager.Get(dataCommandName);
            if (dataCommand == null)
            {
                throw new Errors.DataCommandNotFoundException(dataCommandName);
            }

            var dataCommandInstance = DatabaseInstanceCache.Manager.Get(dataCommand.Database);
            if (dataCommandInstance == null)
            {
                throw new Errors.DatabaseInstanceNotFoundException(dataCommand.Database);
            }

            return GetDataCommand(dataCommandInstance, dataCommand);
        }

        /// <summary>
        /// 根据配置信息获取
        /// </summary>
        public static IDataCommand GetDataCommand(Configuration.DatabaseInstance databaseInstance, Configuration.DataCommand dataCommand)
        {
            DatabaseObject databaseObject = null;
            if (string.IsNullOrEmpty(databaseInstance.Provider))
            {
                databaseObject = new DatabaseObject(SqlClientFactory.Instance, databaseInstance.ConnectionString);
            }
            else
            {
                DbProviderFactory providerFactory = null;
                if (_CachedDbProviderFactories.ContainsKey(databaseInstance.Provider))
                {
                    providerFactory = _CachedDbProviderFactories[databaseInstance.Provider];
                }
                else
                {
                    providerFactory = GetDbProviderFactory(databaseInstance.Provider);
                }

                if (providerFactory == null)
                {
                    throw new Errors.DbProviderNotFoundException(databaseInstance.Provider);
                }

                databaseObject = new DatabaseObject(providerFactory, databaseInstance.ConnectionString);
            }

            var dataCommandObject = new DataCommandObject(databaseObject, dataCommand.CommandType, dataCommand.CommandText);
            if (dataCommand.Parameters != null && dataCommand.Parameters.Length > 0)
            {
                foreach (var parameter in dataCommand.Parameters)
                {
                    dataCommandObject.AddParameter(parameter.Name, parameter.DbType, parameter.Direction, parameter.Size);
                }
            }

            return dataCommandObject;
        }

        private static Dictionary<string, DbProviderFactory> _CachedDbProviderFactories = new Dictionary<string, DbProviderFactory>();

        private static DbProviderFactory GetDbProviderFactory(string providerString)
        {
            Type providerType;
            if (!ReflectionUtility.TryGetType(providerString, out providerType))
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("provider type not found. providerString={0}", providerString));
                return null;
            }

            var providerInstance = providerType.GetField("Instance", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
            if (providerInstance == null)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("provider instance not found. providerString={0}", providerString));
                return null;
            }

            if (!providerInstance.FieldType.IsSubclassOf(typeof(DbProviderFactory)))
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("provider instance is not subclass of DbProviderFactory. providerString={0}", providerString));
                return null;
            }

            var providerFactory = providerInstance.GetValue(null) as DbProviderFactory;
            if (providerFactory == null)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("provider instance is null. providerString={0}", providerString));
                return null;
            }

            if (!_CachedDbProviderFactories.ContainsKey(providerString))
            {
                _CachedDbProviderFactories.Add(providerString, providerFactory);
            }

            return providerFactory;
        }
    }
}