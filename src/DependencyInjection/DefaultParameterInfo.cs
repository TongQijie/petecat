using System.Reflection;

namespace Petecat.DependencyInjection
{
    public class DefaultParameterInfo : IParameterInfo
    {
        public DefaultParameterInfo(ParameterInfo parameterInfo)
        {
            Index = parameterInfo.Position;
            ParameterName = parameterInfo.Name;
            TypeDefinition = new DefaultTypeDefinition(parameterInfo.ParameterType);
        }

        public int Index { get; private set; }

        public string ParameterName { get; private set; }

        public object ParameterValue { get; private set; }

        public ITypeDefinition TypeDefinition { get; private set; }

        public void SetValue(object value)
        {
            ParameterValue = value;
        }
    }
}
