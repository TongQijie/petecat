﻿using System;
using System.Reflection;

using Petecat.DependencyInjection;

namespace Petecat.DynamicProxy
{
    public class InvocationBase : IInvocation
    {
        public void Process()
        {
            ReturnValue = MethodInfo.Invoke(DependencyInjector.GetObject(TargetType), ParameterValues);
        }

        public MethodInfo MethodInfo { get; set; }

        public object[] ParameterValues { get; set; }

        public object ReturnValue { get; set; }

        public Type TargetType { get; set; }

        public Type InterceptorType { get; set; }
    }
}