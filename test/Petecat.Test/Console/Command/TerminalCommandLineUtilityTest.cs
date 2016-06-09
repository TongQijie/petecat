using Microsoft.VisualStudio.TestTools.UnitTesting;

using Petecat.Console.Command;

namespace Petecat.Test.Console.Command
{
    [TestClass]
    public class TerminalCommandLineUtilityTest
    {
        [TestMethod]
        public void Parse()
        {
            var commandText = "create -cat ShoppingCart -id 7 -idx 3 -desc \"Insert GC Items, BOM Items into ShoppingCartContext.ItemSourceList\" -path BizComponents\\Newegg.Biz.OrderManagement\\Newegg.Biz.OrderManagement.ShoppingCart\\ShoppingOrderGenerator\\ShoppingOrderGenerator.cs -text \"var gcItemInfo = inactiveItemDetailInfos.Find(n => string.Equals(n.ItemNumber, \\\"GC-000-001\\\"));\" -line 2214";

            var terminalCommandLine = TerminalCommandLineUtility.Parse(commandText);
        }
    }
}
