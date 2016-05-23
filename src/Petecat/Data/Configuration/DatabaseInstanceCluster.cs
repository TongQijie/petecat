using System;
using System.Text;
using System.Reflection;

namespace Petecat.Data.Configuration
{
    public class DatabaseInstanceCluster
    {
        public DatabaseInstanceCluster()
        {
            DatabaseInstances = new Collection.ThreadSafeKeyedObjectCollection<string, DatabaseInstance>();
        }

        public DatabaseInstanceCluster(DatabaseInstance[] databaseInstances) : this()
        {
            if (databaseInstances != null)
            {
                DatabaseInstances.AddRange(databaseInstances);
            }
        }

        public Collection.ThreadSafeKeyedObjectCollection<string, DatabaseInstance> DatabaseInstances { get; private set; }

        public static DatabaseInstanceCluster Read(string path, Encoding encoding)
        {
            try
            {
                return new DatabaseInstanceCluster(Xml.Serializer.ReadObject<DatabaseInstanceCollection>(path, encoding).DatabaseInstances);
            }
            catch (Exception)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("failed to read databaseInstances from {0}", path));
            }

            return null;
        }

        public void AddRange(string path, Encoding encoding)
        {
            try
            {
                DatabaseInstances.AddRange(Xml.Serializer.ReadObject<DatabaseInstanceCollection>(path, encoding).DatabaseInstances);
            }
            catch (Exception)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("failed to read databaseInstances from {0}", path));
            }
        }
    }
}
