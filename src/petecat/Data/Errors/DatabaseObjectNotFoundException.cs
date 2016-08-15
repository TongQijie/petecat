using System;

namespace Petecat.Data.Errors
{
    public class DatabaseObjectNotFoundException : Exception
    {
        public DatabaseObjectNotFoundException(string databaseObjectName)
            : base(string.Format("Database '{0}' cannot be found.", databaseObjectName))
        {
        }

        public DatabaseObjectNotFoundException(string databaseObjectName, Exception innerException)
            : base(string.Format("Database '{0}' cannot be found.", databaseObjectName), innerException)
        {
        }
    }
}
