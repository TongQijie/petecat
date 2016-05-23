using System.Data;

namespace Petecat.Data.Entity
{
    internal class DataRowEntitySource : DataEntitySourceBase
    {
        private DataRow _DataRow = null;

        public DataRowEntitySource(DataRow dataRow)
        {
            _DataRow = dataRow;
        }

        public override object this[string columnName]
        {
            get { return _DataRow[columnName]; }
        }

        public override object this[int index]
        {
            get { return _DataRow[index]; }
        }

        public override bool ContainsColumn(string columnName)
        {
            return _DataRow.Table.Columns.Contains(columnName);
        }
    }
}