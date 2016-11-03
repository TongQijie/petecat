namespace Petecat.DependencyInjection
{
    public class ConfigurableContainerBase : ContainerBase, IConfigurableContainer
    {
        public override object GetObject(string objectName)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterConfigurationFile(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
