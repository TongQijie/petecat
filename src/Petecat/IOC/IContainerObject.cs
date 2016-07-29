using Petecat.Collection;

namespace Petecat.IoC
{
    public interface IContainerObject : IKeyedObject<string>
    {
        ITypeDefinition TypeDefinition { get; }

        bool IsSingleton { get; }

        object GetObject();
    }
}
