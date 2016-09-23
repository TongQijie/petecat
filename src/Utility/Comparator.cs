namespace Petecat.Utility
{
    public static class Comparator
    {
        public static bool Equal(object first, object second)
        {
            return (first == null && second == null)
                || (first != null && first.Equals(second))
                || (second != null && second.Equals(first));
        }
    }
}
