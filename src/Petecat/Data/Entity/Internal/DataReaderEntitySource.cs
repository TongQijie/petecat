using System.Data;

namespace Petecat.Data.Entity
{
    internal class DataReaderEntitySource : DataEntitySourceBase
    {
        private IDataReader _DataReader = null;

        public DataReaderEntitySource(IDataReader dataReader)
        {
            _DataReader = dataReader;
        }

        public override object this[string columnName]
        {
            get { return _DataReader[columnName]; }
        }

        public override object this[int index]
        {
            get { return _DataReader[index]; }
        }

        public override bool ContainsColumn(string columnName)
        {
            var schemaTable = _DataReader.GetSchemaTable();
            foreach (DataRow row in schemaTable.Rows)
            {
                if (string.Compare(row["ColumnName"].ToString().Trim(), columnName.Trim(), true) == 0)
                {
                    return true;
                }
            }

            return false;
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