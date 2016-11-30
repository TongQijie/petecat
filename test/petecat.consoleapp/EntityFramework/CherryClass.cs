using Petecat.EntityFramework.Attribute;
namespace Petecat.ConsoleApp.EntityFramework
{
    public class CherryClass
    {
        [SimpleValue(ColumnName = "ItemNumber")]
        public string ItemNumber { get; set; }

        [CompositeValue(Prefix = "Apple")]
        public AppleClass AppleClass { get; set; }
    }
}
