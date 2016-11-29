namespace Petecat.EntityFramework
{
    public interface IDatabaseProvider
    {
        IDatabase Get(string name);
    }
}
