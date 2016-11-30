using Petecat.EntityFramework.Attribute;
using System;
namespace Petecat.ConsoleApp.EntityFramework
{
    public class AppleClass
    {
        [SimpleValue(ColumnName = "SONumber")]
        public int Number { get; set; }

        [SimpleValue(ColumnName = "SODate")]
        public DateTime Date { get; set; }

        [SimpleValue(ColumnName = "SOMemo")]
        public string Memo { get; set; }
    }
}