using System;

namespace Petecat.Data.Entity
{
    internal abstract class DataEntitySourceBase : IEntityDataSource
    {
        public abstract object this[int index] { get; }

        public abstract object this[string columnName] { get; }

        public abstract bool ContainsColumn(string columnName);

        public virtual void Dispose()
        {
        }

        public virtual object GetColumnValue(string columnName, Type targetType)
        {
            if (!ContainsColumn(columnName))
            {
                return null;
            }

            if (this[columnName] == DBNull.Value)
            {
                if (targetType.IsValueType)
                {
                    return Activator.CreateInstance(targetType);
                }
                else
                {
                    return null;
                }
            }

            if (targetType == typeof(string))
            {
                return this[columnName].ToString().Trim();
            }
            else if (targetType.IsEnum)
            {
                return Enum.ToObject(targetType, this[columnName]);
            }
            else
            {
                return Convert.ChangeType(this[columnName], targetType);
            }
        }
    }
}
