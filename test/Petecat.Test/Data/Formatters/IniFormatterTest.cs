using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.Data.Formatters;
using System.Text;

namespace Petecat.Test.Data.Formatters
{
    [TestClass]
    public class IniFormatterTest
    {
        [TestMethod]
        public void Deserialize_Test()
        {
            var itemInfo = new IniFormatter().ReadObject<ItemInfo>("iteminfo.ini");
        }
    }
}
