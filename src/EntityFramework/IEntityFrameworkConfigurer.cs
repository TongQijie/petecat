using Petecat.EntityFramework.Configuration;

namespace Petecat.EntityFramework
{
    public interface IEntityFrameworkConfigurer
    {
        DatabaseCommandItemConfiguration GetDatabaseCommandItem(string name);
    }
}
