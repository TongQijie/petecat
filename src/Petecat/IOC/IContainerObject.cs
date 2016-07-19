using Petecat.Collection;

namespace Petecat.IOC
{
    public interface IContainerObject : IKeyedObject<string>
    {
        ITypeDefinition TypeDefinition { get; }

        bool IsSingleton { get; }

        object GetObject();
    }
}
