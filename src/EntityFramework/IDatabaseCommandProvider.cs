namespace Petecat.EntityFramework
{
    public interface IDatabaseCommandProvider
    {
        IDatabaseCommand GetDatabaseCommand(string name);
    }
}
