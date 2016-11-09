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

        public IParameterInfo[] ParameterInfos { get; protected set; }

        public IPropertyInfo[] PropertyInfos { get; protected set; }

        private object _SingletonInstance = null;

        public object GetInstance()
        {
            if (Singleton)
            {
                if (_SingletonInstance == null)
                {
                    // TODO
                }

                return _SingletonInstance;
            }
            else
            {
                // TODO
            }

            return null;
        }
    }
}