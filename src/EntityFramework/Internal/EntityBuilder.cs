using System;
using System.Linq;
using System.Data;
using System.Collections.Concurrent;

using Petecat.Utility;
using Petecat.EntityFramework.Attribute;

namespace Petecat.EntityFramework.Internal
{
    internal class EntityBuilder
    {
        private EntityBuilder(Type entityType)
        {
            EntityType = entityType;
            Initialize();
        }

        public Type EntityType { get; private set; }

        #region Property Infos

        private ConcurrentDictionary<string, SimpleValuePropertyInfo> _SimpleValuePropertyInfos = null;

        public ConcurrentDictionary<string, SimpleValuePropertyInfo> SimpleValuePropertyInfos
        {
            get { return _SimpleValuePropertyInfos ?? (_SimpleValuePropertyInfos = new ConcurrentDictionary<string, SimpleValuePropertyInfo>()); }
        }

        private ConcurrentBag<CompositeValuePropertyInfo> _CompositeValuePropertyInfos = null;

        public ConcurrentBag<CompositeValuePropertyInfo> CompositeValuePropertyInfos
        {
            get { return _CompositeValuePropertyInfos ?? (_CompositeValuePropertyInfos = new ConcurrentBag<CompositeValuePropertyInfo>()); }
        }

        private object _InitLock = new object();

        private bool _IsInitialized = false;

        private void Initialize()
        {
            if (_IsInitialized)
            {
                return;
            }

            lock (_InitLock)
            {
                if (_IsInitialized)
                {
                    return;
                }

                foreach (var propertyInfo in EntityType.GetProperties().Where(x => x.CanRead && x.CanWrite))
                {
                    SimpleValueAttribute attr1;
                    if (Reflector.TryGetCustomAttribute(propertyInfo, null, out attr1))
                    {
                        var simpleValuePropertyInfo = new SimpleValuePropertyInfo(propertyInfo, attr1);
                        SimpleValuePropertyInfos.TryAdd(simpleValuePropertyInfo.ColumnName, simpleValuePropertyInfo);
                        continue;
                    }

                    CompositeValueAttribute attr2;
                    if (Reflector.TryGetCustomAttribute(propertyInfo, null, out attr2))
                    {
                        var compositeValuePropertyInfo = new CompositeValuePropertyInfo(propertyInfo, attr2);
                        CompositeValuePropertyInfos.Add(compositeValuePropertyInfo);
                        continue;
                    }
                }

                _IsInitialized = true;
            }
        }

        #endregion

        #region EntityBuilder Cache

        private static ConcurrentDictionary<Type, EntityBuilder> _CachedEntityBuilders = null;

        private static ConcurrentDictionary<Type, EntityBuilder> CachedEntityBuilders
        {
            get { return _CachedEntityBuilders ?? (_CachedEntityBuilders = new ConcurrentDictionary<Type, EntityBuilder>()); }
        }

        public static EntityBuilder GetBuilder(Type entityType)
        {
            if (!CachedEntityBuilders.ContainsKey(entityType))
            {
                CachedEntityBuilders.TryAdd(entityType, new EntityBuilder(entityType));
            }

            EntityBuilder entityBuilder;
            if (CachedEntityBuilders.TryGetValue(entityType, out entityBuilder))
            {
                return entityBuilder;
            }

            return null;
        }

        #endregion

        public object Build(IDataReader dataReader, string prefix = null)
        {
            return InternalBuild(new DataReaderEntityDataSource(dataReader), prefix);
        }

        private object InternalBuild(IEntityDataSource entityDataSource, string prefix = null)
        {
            var instance = Activator.CreateInstance(EntityType);

            foreach (var kvp in SimpleValuePropertyInfos)
            {
                var columnName = prefix == null ? kvp.Value.ColumnName : (prefix + "_" + kvp.Value.ColumnName);
                if (!entityDataSource.ContainsColumn(columnName))
                {
                    continue;
                }

                kvp.Value.PropertyInfo.SetValue(instance, entityDataSource.GetValue(columnName, kvp.Value.PropertyInfo.PropertyType), null);
            }

            foreach (var prop in CompositeValuePropertyInfos)
            {
                var builder = GetBuilder(prop.PropertyInfo.PropertyType);
                prop.PropertyInfo.SetValue(instance, builder.InternalBuild(entityDataSource, prop.Prefix), null);
            }

            return instance;
        }
    }
}