using System;

namespace Petecat.DependencyInjection
{
    public interface IContainer
    {
        void RegisterInstance(IInstanceInfo instanceInfo);

        void RegisterType(ITypeDefinition typeDefinition);

        bool ContainsType(Type objectType);

        object GetObject(Type objectType);

        T GetObject<T>();

        bool ContainsInstance(string objectName);

        object GetObject(string objectName);

        T GetObject<T>(string objectName);
    }
}
