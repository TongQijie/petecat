using System;

namespace Petecat.Data.Errors
{
    public class DataCommandNotFoundException : Exception
    {
        public DataCommandNotFoundException(string dataCommandName)
            : base(string.Format("DataCommand({0}) not found.", dataCommandName))
        {
        }

        public DataCommandNotFoundException(string dataCommandName, Exception innerException)
            : base(string.Format("DataCommand({0}) not found.", dataCommandName), innerException)
        {
        }
    }
}
