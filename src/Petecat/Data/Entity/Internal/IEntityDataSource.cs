using System;

namespace Petecat.Data.Entity
{
    internal interface IEntityDataSource : IDisposable
    {
        object this[string columnName]
        {
            get;
        }

        object this[int index]
        {
            get;
        }

        bool ContainsColumn(string columnName);

        object GetColumnValue(string columnName, Type targetType);
    }
}