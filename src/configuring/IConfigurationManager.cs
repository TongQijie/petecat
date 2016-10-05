namespace Petecat.Configuring
{
    public interface IConfigurationManager
    {
        T GetValue<T>(string key);
    }
}

