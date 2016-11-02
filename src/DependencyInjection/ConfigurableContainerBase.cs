namespace Petecat.DependencyInjection
{
    public class ConfigurableContainerBase : ContainerBase, IConfigurableContainer
    {
        public object GetObject(string objectName)
        {
            throw new System.NotImplementedException();
        }

        public T GetObject<T>(string objectName)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterConfigurationFile(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
