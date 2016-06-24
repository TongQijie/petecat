using System;

namespace Petecat.Data.Errors
{
    public class DatabaseInstanceNotFoundException : Exception
    {
        public DatabaseInstanceNotFoundException(string databaseInstanceName)
            : base(string.Format("Database Instance({0}) not found.", databaseInstanceName))
        {
        }

        public DatabaseInstanceNotFoundException(string databaseInstanceName, Exception innerException)
            : base(string.Format("Database Instance({0}) not found.", databaseInstanceName), innerException)
        {
        }
    }
}
