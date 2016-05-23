using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data;

using Petecat.Data.Xml;
using Petecat.Data.Configuration;
using System.Text;

namespace Petecat.Test.Data.Configuration
{
    [TestClass]
    public class DataOperationClusterTest
    {
        [TestMethod]
        public void Read()
        {
            var dataCommandCluster = DataOperationCluster.Read("configuration/operations.config", Encoding.UTF8);
            Assert.IsNotNull(dataCommandCluster);
        }

        [TestMethod]
        public void Write()
        {
            var dataOperationCommands = new DataOperation[]
            {
                new DataOperation() { Key = "SQL-01", Database = "sql server", CommandText = "this is a raw command", CommandType = CommandType.Text },
                new DataOperation() { Key = "SQL-02", Database = "mysql", CommandText = "this is a sp command", CommandType = CommandType.StoredProcedure,
                    Parameters = new DataOperationParameter[]
                    {
                        new DataOperationParameter() { Name = "Param-01", DbType = DbType.AnsiString, Direction = ParameterDirection.Input },
                        new DataOperationParameter() { Name = "Param-02", DbType = DbType.AnsiString, Direction = ParameterDirection.Input },
                        new DataOperationParameter() { Name = "Param-03", DbType = DbType.AnsiString, Direction = ParameterDirection.Output },
                    }
                },
                new DataOperation() { Key = "SQL-03", Database = "oracle", CommandText = "this is a sp command", CommandType = CommandType.StoredProcedure,
                    Parameters = new DataOperationParameter[]
                    {
                        new DataOperationParameter() { Name = "Param-01", DbType = DbType.AnsiString, Direction = ParameterDirection.Input },
                    }
                },
            };
            var dataOperationCommandCollection = new DataOperationCollection() { DataOperationCommands = dataOperationCommands };
            Serializer.WriteObject(dataOperationCommandCollection, "configuration/operations.config", Encoding.UTF8);
        }
    }
}
