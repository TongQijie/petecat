using Petecat.Aop;
using System;

namespace Petecat.Test.Aop
{
    public class AopAppleBase : AppleBase
    {
        public AopAppleBase(IAopInterceptor aopInterceptor)
        {
            _AopInterceptor = aopInterceptor;
        }

        private IAopInterceptor _AopInterceptor = null;

        public override string SayHi(string welcome)
        {
            var aopInvocation = new DefaultAopInvocation();
            var baseClass = new AppleBase();
            aopInvocation.Owner = baseClass;
            aopInvocation.Method = typeof(AppleBase).GetMethod("SayHi");
            aopInvocation.ParameterValues = new object[] { welcome };
            _AopInterceptor.Intercept(aopInvocation);
            return aopInvocation.ReturnValue as string;
        }

        public override string SayTo(string welcome, string to)
        {
            var aopInvocation = new DefaultAopInvocation();
            var baseClass = new AppleBase();
            aopInvocation.Owner = baseClass;
            aopInvocation.Method = typeof(AppleBase).GetMethod("SayTo", new Type[] { typeof(string), typeof(string) });
            aopInvocation.ParameterValues = new object[] { welcome, to };
            _AopInterceptor.Intercept(aopInvocation);
            return aopInvocation.ReturnValue as string;
        }

        public override void KeepSilent(string welcome)
        {
            var aopInvocation = new DefaultAopInvocation();
            var baseClass = new AppleBase();
            aopInvocation.Owner = baseClass;
            aopInvocation.Method = typeof(AppleBase).GetMethod("KeepSilent");
            aopInvocation.ParameterValues = new object[] { welcome };
            _AopInterceptor.Intercept(aopInvocation);
        }
    }
}
