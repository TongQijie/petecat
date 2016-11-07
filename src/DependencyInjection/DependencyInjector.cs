using System;
using System.Linq;

using Petecat.Extension;

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

        public static void Setup(IContainer container)
        {
            _Containers = _Containers.Append(container);
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
    }
}
