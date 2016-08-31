using Petecat.Collection;

namespace Petecat.IoC
{
    public interface IIoCObject : IKeyedObject<string>
    {
        ITypeDefinition TypeDefinition { get; }

        bool IsSingleton { get; }

        object GetObject();
    }
}
