namespace Petecat.DependencyInjection
{
    public interface IContainer
    {
        void RegisterInstance(IInstanceInfo instanceInfo);

        void RegisterType(ITypeDefinition typeDefinition);
    }
}
