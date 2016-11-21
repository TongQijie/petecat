namespace Petecat.EntityFramework.Configuration
{
    public interface IDatabaseCommandConfiguration
    {
        DatabaseCommandItemConfiguration[] Commands { get; }
    }
}
