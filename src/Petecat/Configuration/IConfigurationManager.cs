namespace Petecat.Configuration
{
    public interface IConfigurationManager
    {
        bool EnableCache { get; }

        void Set(string key, object value);

        T Get<T>(string key);

        T Get<T>(string key, T defaultValue);
    }
}