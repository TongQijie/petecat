using System;
using System.Reflection;
using System.Text;

namespace Petecat.Data.Configuration
{
    public class DataOperationCluster
    {
        public DataOperationCluster()
        {
            DataOperations = new Collection.ThreadSafeKeyedObjectCollection<string, DataOperation>();
        }

        public DataOperationCluster(DataOperation[] dataOperationCommands) : this()
        {
            if (dataOperationCommands != null)
            {
                DataOperations.AddRange(dataOperationCommands);
            }
        }

        public Collection.ThreadSafeKeyedObjectCollection<string, DataOperation> DataOperations { get; private set; }

        public static DataOperationCluster Read(string path, Encoding encoding)
        {
            try
            {
                return new DataOperationCluster(Xml.Serializer.ReadObject<DataOperationCollection>(path, encoding).DataOperationCommands);
            }
            catch (Exception)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("failed to read DataOperations from {0}", path));
            }

            return null;
        }

        public void AddRange(string path, Encoding encoding)
        {
            try
            {
                DataOperations.AddRange(Xml.Serializer.ReadObject<DataOperationCollection>(path, encoding).DataOperationCommands);
            }
            catch (Exception)
            {
                Logging.LoggerManager.Get().LogEvent(Assembly.GetExecutingAssembly().FullName, Logging.LoggerLevel.Error, string.Format("failed to read DataOperations from {0}", path));
            }
        }
    }
}
