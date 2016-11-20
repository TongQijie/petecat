namespace Petecat.EntityFramework.Attribute
{
    using System;

    public class SimpleValueAttribute : Attribute
    {
        public string ColumnName { get; set; }
    }
}
