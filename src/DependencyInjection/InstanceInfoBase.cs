using System;
using System.Linq;
using System.Reflection;

namespace Petecat.DependencyInjection
{
    public class InstanceInfoBase : IInstanceInfo
    {
        public InstanceInfoBase(string name, ITypeDefinition typeDefinition, bool singleton)
        {
            Name = name;
            TypeDefinition = typeDefinition;
            Singleton = singleton;
        }

        public string Name { get; private set; }

        public bool Singleton { get; private set; }

        public ITypeDefinition TypeDefinition { get; private set; }

        public IParameterInfo[] ParameterInfos { get; set; }

        public IPropertyInfo[] PropertyInfos { get; set; }

        private object _SingletonInstance = null;

        public object GetInstance()
        {
            if (Singleton)
            {
                if (_SingletonInstance == null)
                {
                    _SingletonInstance = CreateInstance();
                }

                return _SingletonInstance;
            }
            else
            {
                return CreateInstance();
            }
        }

        private object CreateInstance()
        {
            var instance = Activator.CreateInstance(TypeDefinition.Info as Type,
                ParameterInfos == null ? new object[0] : ParameterInfos.OrderBy(x => x.Index).Select(x => x.ParameterValue).ToArray());

            if (PropertyInfos != null && PropertyInfos.Length > 0)
            {
                foreach (var propertyInfo in PropertyInfos)
                {
                    (propertyInfo.PropertyDefinition.Info as PropertyInfo).SetValue(instance, propertyInfo.PropertyValue);
                }
            }

            return instance;
        }
    }
}