using Petecat.EntityFramework.Attribute;
namespace Petecat.ConsoleApp.EntityFramework
{
    public class BananaClass
    {
        [SimpleValue(ColumnName = "ItemNumber")]
        public string ItemNumber { get; set; }

        [SimpleValue(ColumnName = "UnitPrice")]
        public decimal UnitPrice { get; set; }

        [SimpleValue(ColumnName = "Quantity")]
        public int Quantity { get; set; }
    }
}
