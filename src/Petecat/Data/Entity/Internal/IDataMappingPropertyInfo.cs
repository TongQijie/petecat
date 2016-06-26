using System.Reflection;

namespace Petecat.Data.Entity
{
    internal interface IDataMappingPropertyInfo : Collection.IKeyedObject<string>
    {
        PropertyInfo PropertyInfo { get; }
    }
}
