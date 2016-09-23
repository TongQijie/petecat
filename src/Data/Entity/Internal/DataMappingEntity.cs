using System;

namespace Petecat.Data.Entity
{
    internal class DataMappingEntity : Collection.IKeyedObject<string>
    {
        public DataMappingEntity(Type type)
        {
            Type = type;
            DataMappingPropertyInfos = new Collection.ThreadSafeKeyedObjectCollection<string, IDataMappingPropertyInfo>();
        }

        public Type Type { get; private set; }

        public Collection.ThreadSafeKeyedObjectCollection<string, IDataMappingPropertyInfo> DataMappingPropertyInfos { get; private set; }
        
        public string Key { get { return Type.FullName; } }
    }
}