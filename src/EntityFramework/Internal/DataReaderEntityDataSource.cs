using System.Data;
using System.Collections.Generic;

namespace Petecat.EntityFramework.Internal
{
    internal class DataReaderEntityDataSource : EntityDataSourceBase
    {
        public DataReaderEntityDataSource(IDataReader dataReader)
        {
            _DataReader = dataReader;
        }

        private IDataReader _DataReader = null;

        public override object this[string columnName]
        {
            get { return _DataReader[columnName]; }
        }

        public override object this[int index]
        {
            get { return _DataReader[index]; }
        }

        private List<string> _ColumnNames = null;

        public override bool ContainsColumn(string columnName)
        {
            if (_ColumnNames == null)
            {
                var schemaTable = _DataReader.GetSchemaTable();
                var columnNames = new List<string>();
                foreach (DataRow row in schemaTable.Rows)
                {
                    columnNames.Add(row["ColumnName"].ToString().Trim());
                }
                _ColumnNames = columnNames;
            }

            return _ColumnNames.Contains(columnName.Trim());
        }

        public override void Dispose()
        {
            if (_DataReader != null)
            {
                _DataReader.Dispose();
            }
        }
    }
}
