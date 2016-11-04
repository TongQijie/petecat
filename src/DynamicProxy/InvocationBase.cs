using System.Reflection;

namespace Petecat.DynamicProxy
{
    public class InvocationBase : IInvocation
    {
        public void Process()
        {
            ReturnValue = MethodInfo.Invoke(Owner, ParameterValues);
        }

        public MethodInfo MethodInfo { get; set; }

        public object[] ParameterValues { get; set; }

        public object ReturnValue { get; set; }

        public object Owner { get; set; }
    }
}
