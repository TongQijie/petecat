namespace Petecat.DependencyInjection
{
    public interface IConfigurableContainer : IContainer
    {
        void RegisterConfigurableFile(IConfigurableFileInfo configurableFileInfo);
    }
}