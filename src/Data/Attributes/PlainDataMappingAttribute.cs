using System.Data;

namespace Petecat.Data.Attributes
{
    public class PlainDataMappingAttribute : DataMappingAttributeBase
    {
        public PlainDataMappingAttribute()
        {
        }

        public PlainDataMappingAttribute(string columnName)
        {
            ColumnName = columnName;
        }

        public PlainDataMappingAttribute(string columnName, DbType dataType) : this(columnName)
        {
            DbType = dataType;
        }

        public string ColumnName { get; private set; }

        public DbType DbType { get; private set; }
    }
}