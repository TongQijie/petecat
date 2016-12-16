using System;
using System.Linq;

using Petecat.Extending;

namespace Petecat.DependencyInjection
{
    public static class DependencyInjector
    {
        private static IContainer[] _Containers = null;

        public static IContainer[] Containers { get { return _Containers ?? (_Containers = new IContainer[0]); } }

        static DependencyInjector()
        {
            _Containers = new IContainer[0];
        }

        public static T Setup<T>(T container) where T : IContainer
        {
            _Containers = _Containers.Append(container);
            return container;
        }

        public static TContainer GetContainer<TContainer>() where TContainer : IContainer
        {
            return (TContainer)Containers.ToArray().FirstOrDefault(x => x is TContainer);
        }

        public static object GetObject<TContainer>(Type targetType) where TContainer : IAssemblyContainer
        {
            foreach (TContainer assemblyContainer in Containers.Where(x => x is TContainer).ToArray())
            {
                if (assemblyContainer.CanInfer(targetType))
                {
                    return assemblyContainer.GetObject(targetType);
                }
            }

            return null;
        }

        public static TObject GetObject<TContainer, TObject>() where TContainer : IAssemblyContainer
        {
            return (TObject)GetObject<TContainer>(typeof(TObject));
        }

        public static object GetObject(Type targetType)
        {
            return GetObject<IAssemblyContainer>(targetType);
        }

        public static TObject GetObject<TObject>()
        {
            return (TObject)GetObject(typeof(TObject));
        }

        public static object GetObject<TContainer>(string objectName) where TContainer : IConfigurableContainer
        {
            foreach (TContainer configurableContainer in Containers.Where(x => x is TContainer).ToArray())
            {
                if (configurableContainer.ContainsInstance(objectName))
                {
                    return configurableContainer.GetObject(objectName);
                }
            }

            return null;
        }

        public static object GetObject(string objectName)
        {
            return GetObject<IConfigurableContainer>(objectName);
        }
    }
}
