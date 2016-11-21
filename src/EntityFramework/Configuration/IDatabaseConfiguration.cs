namespace Petecat.EntityFramework.Configuration
{
    public interface IDatabaseConfiguration
    {
        DatabaseItemConfiguration[] Databases { get; }
    }
}