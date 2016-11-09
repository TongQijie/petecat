using System.Reflection;

namespace Petecat.DependencyInjection
{
    public class ParameterInfoBase : IParameterInfo
    {
        public ParameterInfoBase()
        {
        }

        public ParameterInfoBase(ParameterInfo parameterInfo)
        {
            Index = parameterInfo.Position;
            ParameterName = parameterInfo.Name;
            TypeDefinition = new TypeDefinitionBase(parameterInfo.ParameterType);
        }

        public int Index { get; set; }

        public string ParameterName { get; set; }

        public object ParameterValue { get; set; }

        public ITypeDefinition TypeDefinition { get; set; }

        public void SetValue(object value)
        {
            ParameterValue = value;
        }
    }
}
