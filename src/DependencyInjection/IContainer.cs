using System;
namespace Petecat.DependencyInjection
{
    public interface IContainer
    {
        void RegisterInstance(IInstanceInfo instanceInfo);

        void RegisterType(ITypeDefinition typeDefinition);

        object GetObject(Type objectType);

        T GetObject<T>();

        object GetObject(string objectName);

        T GetObject<T>(string objectName);
    }
}
