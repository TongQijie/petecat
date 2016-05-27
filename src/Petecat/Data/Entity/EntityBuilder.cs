using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Petecat.Data.Entity
{
    public static class EntityBuilder
    {
        #region 实体对象映射缓存管理

        private static Collection.ThreadSafeKeyedObjectCollection<string, DataMappingEntity> _DataMappingEntities = new Collection.ThreadSafeKeyedObjectCollection<string, DataMappingEntity>();

        private static DataMappingEntity GetDataMappingEntity(Type entityType)
        {
            if (_DataMappingEntities.ContainsKey(entityType.FullName))
            {
                return _DataMappingEntities[entityType.FullName];
            }

            var addedDataMappingEntity = new DataMappingEntity(entityType);

            var propertyInfos = entityType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                var dataMappingAttribute = propertyInfo.GetCustomAttributes(typeof(DataMappingAttributeBase), false).FirstOrDefault();
                if (dataMappingAttribute == null)
                {
                    continue;
                }

                if (dataMappingAttribute is PlainDataMappingAttribute)
                {
                    addedDataMappingEntity.DataMappingPropertyInfos.Add(new PlainDataMappingPropertyInfo(propertyInfo, dataMappingAttribute as PlainDataMappingAttribute));
                }
                else if (dataMappingAttribute is CompositeDataMappingAttribute)
                {
                    addedDataMappingEntity.DataMappingPropertyInfos.Add(new CompositeDataMappingPropertyInfo(propertyInfo, dataMappingAttribute as CompositeDataMappingAttribute));
                }
                else
                {
                    continue;
                }
            }

            _DataMappingEntities.Add(addedDataMappingEntity);

            return addedDataMappingEntity;
        }

        #endregion

        #region 公共方法，用于构造实体对象

        public static T BuildEntity<T>(IDataReader dataReader) where T : class, new()
        {
            return FillEntityProperties(new DataReaderEntitySource(dataReader), typeof(T)) as T;
        }

        public static T BuildEntity<T>(DataRow dataRow) where T : class, new()
        {
            return FillEntityProperties(new DataRowEntitySource(dataRow), typeof(T)) as T;
        }

        #endregion

        #region 私有方法，填充实体对象属性

        private static object FillEntityProperties(IEntityDataSource entityDataSource, Type entityType)
        {
            var dataMappingEntity = GetDataMappingEntity(entityType);
            if (dataMappingEntity == null)
            {
                return null;
            }

            var filledEntity = Activator.CreateInstance(entityType);

            foreach (var dataMappingPropertyInfo in dataMappingEntity.DataMappingPropertyInfos.Values)
            {
                if (dataMappingPropertyInfo is PlainDataMappingPropertyInfo)
                {
                    if (!entityDataSource.ContainsColumn(dataMappingPropertyInfo.Key))
                    {
                        continue;
                    }

                    var plainDataMappingPropertyInfo = dataMappingPropertyInfo as PlainDataMappingPropertyInfo;
                    plainDataMappingPropertyInfo.PropertyInfo.SetValue(filledEntity, entityDataSource.GetColumnValue(dataMappingPropertyInfo.Key, plainDataMappingPropertyInfo.PropertyInfo.PropertyType), null);
                }
                else if (dataMappingPropertyInfo is CompositeDataMappingPropertyInfo)
                {
                    var compositeDataMappingPropertyInfo = dataMappingPropertyInfo as CompositeDataMappingPropertyInfo;
                    compositeDataMappingPropertyInfo.PropertyInfo.SetValue(filledEntity, FillEntityProperties(entityDataSource, compositeDataMappingPropertyInfo.CompositeDataMappingAttribute.Type), null);
                }
                else
                {
                    continue;
                }
            }

            return filledEntity;
        }

        #endregion
    }
}