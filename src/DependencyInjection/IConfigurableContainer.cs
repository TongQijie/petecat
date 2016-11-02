namespace Petecat.DependencyInjection
{
    public interface IConfigurableContainer : IContainer
    {
        object GetObject(string objectName);

        T GetObject<T>(string objectName);

        void RegisterConfigurationFile(string path);
    }
}