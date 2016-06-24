using System;

namespace Petecat.Data.Errors
{
    public class DbProviderNotFoundException : Exception
    {
        public DbProviderNotFoundException(string dbProviderString)
            : base(string.Format("DbProvider({0}) not found.", dbProviderString))
        {
        }

        public DbProviderNotFoundException(string dbProviderString, Exception innerException)
            : base(string.Format("DbProvider({0}) not found.", dbProviderString), innerException)
        {
        }
    }
}
