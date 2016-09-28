using System.Reflection;

namespace Petecat.Aop
{
    public class DefaultAopInvocation : IAopInvocation
    {
        public void Process()
        {
            ReturnValue = Method.Invoke(Owner, ParameterValues);
        }

        public MethodInfo Method { get; set; }

        public object[] ParameterValues { get; set; }

        public object ReturnValue { get; set; }

        public object Owner { get; set; }
    }
}
