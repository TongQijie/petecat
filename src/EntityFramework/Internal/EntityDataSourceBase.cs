using System;

using Petecat.Extending;

namespace Petecat.EntityFramework.Internal
{
    internal abstract class EntityDataSourceBase : IEntityDataSource
    {
        public abstract object this[int index] { get; }

        public abstract object this[string columnName] { get; }

        public abstract bool ContainsColumn(string columnName);

        public object GetValue(string columnName, Type targetType)
        {
            if (!ContainsColumn(columnName))
            {
                // TODO: throw
            }

            if (this[columnName] == DBNull.Value)
            {
                return targetType.GetDefaultValue();
            }

            if (targetType == typeof(string))
            {
                return this[columnName].ToString().Trim();
            }

            if (targetType.IsEnum)
            {
                return Enum.ToObject(targetType, this[columnName]);
            }

            return Convert.ChangeType(this[columnName], targetType);
        }

        public virtual void Dispose()
        {
        }
    }
}
