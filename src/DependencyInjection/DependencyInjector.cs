using System;

using Petecat.Extension;
using Petecat.DependencyInjection.Containers;

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

        public static object GetObject<TContainer>(Type targetType) where TContainer : IContainer
        {
            var assemblyContainer = Containers.FirstOrDefault(x => x is TContainer);
            if (assemblyContainer == null)
            {
                // TODO: throw
            }

            return ((TContainer)assemblyContainer).GetObject(targetType);
        }

        public static TObject GetObject<TContainer, TObject>() where TContainer : IContainer
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
