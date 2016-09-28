using Petecat.Aop;
namespace Petecat.Test.Aop
{
    public class AopAppleBase : AppleBase
    {
        public AopAppleBase(IAopInterceptor aopInterceptor)
        {
            _AopInterceptor = aopInterceptor;
        }

        private IAopInterceptor _AopInterceptor = null;

        public override object SayHi(object welcome)
        {
            var aopInvocation = new DefaultAopInvocation();
            var baseClass = new AppleBase();
            var invocationType = typeof(DefaultAopInvocation);
            invocationType.GetMethod("set_Owner").Invoke(aopInvocation, new object[] { baseClass });
            invocationType.GetMethod("set_Method").Invoke(aopInvocation, new object[] { baseClass.GetType().GetMethod("SayHi") });
            invocationType.GetMethod("set_ParameterValues").Invoke(aopInvocation, new object[] { new object[] { welcome } });
            this._AopInterceptor.Intercept(aopInvocation);
            return aopInvocation.ReturnValue;
        }
    }
}
