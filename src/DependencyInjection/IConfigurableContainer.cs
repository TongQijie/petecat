namespace Petecat.DependencyInjection
{
    public interface IConfigurableContainer : IContainer
    {
        void RegisterConfigurationFile(string path);
    }
}