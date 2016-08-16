using System;
using System.Linq;

using Petecat.Utility;

namespace Petecat.IoC
{
    public class DefaultContainerObject : IContainerObject
    {
        public DefaultContainerObject(string name, bool singleton, ITypeDefinition typeDefinition)
        {
            Key = name;
            IsSingleton = singleton;
            TypeDefinition = typeDefinition;
        }

        public string Key { get; private set; }

        public ITypeDefinition TypeDefinition { get; private set; }

        public MethodArgument[] Arguments { get; set; }

        public InstanceProperty[] Properties { get; set; }

        public bool IsSingleton { get; private set; }

        public object _Singleton = null;

        public object GetObject()
        {
            if (IsSingleton)
            {
                if (_Singleton == null)
                {
                    _Singleton = InternalGetObject();
                }

                return _Singleton;
            }
            else
            {
                return InternalGetObject();
            }
        }

        private object InternalGetObject()
        {
            if (Arguments == null || Arguments.Length == 0)
            {
                var instance = TypeDefinition.GetInstance();

                if (Properties != null && Properties.Length > 0)
                {
                    foreach (var property in Properties)
                    {
                        var propertyDefinition = TypeDefinition.Properties.FirstOrDefault(x => x.PropertyName == property.Name);
                        if (propertyDefinition == null)
                        {
                            return null;
                        }

                        propertyDefinition.SetValue(instance, property.PropertyValue);
                    }
                }

                return instance;
            }
            else
            {
                foreach (var constructor in TypeDefinition.Constructors)
                {
                    object[] arguments;
                    if (constructor.TryGetArgumentValues(Arguments, out arguments))
                    {
                        var instance = TypeDefinition.GetInstance(arguments);

                        if (Properties != null && Properties.Length > 0)
                        {
                            foreach (var property in Properties)
                            {
                                var propertyDefinition = TypeDefinition.Properties.FirstOrDefault(x => x.PropertyName == property.Name);
                                if (propertyDefinition == null)
                                {
                                    return null;
                                }

                                propertyDefinition.SetValue(instance, property.PropertyValue);
                            }
                        }

                        return instance;
                    }
                }

                return null;
            }
        }
    }
}