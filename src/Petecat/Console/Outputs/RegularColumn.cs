namespace Petecat.Console.Outputs
{
    public class RegularColumn
    {
        public RegularColumn(int index, int length)
        {
            Index = index;
            Length = length;
        }

        public int Index { get; set; }

        public int Length { get; set; }

        public string Value { get; set; }
    }
}
