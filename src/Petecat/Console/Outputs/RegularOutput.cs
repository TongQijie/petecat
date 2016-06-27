using System.Collections.Generic;
using System.Linq;

namespace Petecat.Console.Outputs
{
    using System;

    public class RegularOutput
    {
        private List<RegularColumn> _Columns = null;

        public List<RegularColumn> Columns { get { return _Columns ?? (_Columns = new List<RegularColumn>()); } }

        public void OutputColumn(int index, string value, bool newline = false)
        {
            var column = Columns.FirstOrDefault(x => x.Index == index);
            if (column == null)
            {
                return;
            }

            column.Value = value;

            Refresh();

            if (newline)
            {
                OutputNewLine();
            }
        }

        public void OutputNewLine()
        {
            Columns.ForEach(x => x.Value = string.Empty);

            CommonUtility.WriteNewLine();
        }

        public void Refresh()
        {
            Console.Write("\r");

            if (Columns.Count == 0)
            {
                return;
            }

            foreach (var column in Columns.OrderBy(x => x.Index))
            {
                var formatString = "";
                if (column.Index == Columns.Min(x => x.Index))
                {
                    formatString = @"{0,-" + column.Length + "}";
                }
                else
                {
                    formatString = @"{0," + column.Length + "}";
                }
                
                if (string.IsNullOrEmpty(column.Value))
                {
                    if (Columns.Exists(x => x.Index > column.Index && !string.IsNullOrEmpty(x.Value)))
                    {
                        Console.Write(formatString, "");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Console.Write(formatString, column.Value.Substring(0, Math.Min(column.Length, column.Value.Length)));
                }
            }
        }
    }
}
