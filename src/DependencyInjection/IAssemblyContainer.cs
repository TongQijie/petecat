﻿using System;

namespace Petecat.DependencyInjection
{
    public interface IAssemblyContainer : IContainer
    {
        void RegisterAssembly(IAssemblyInfo assemblyInfo);

        bool CanInfer(Type targetType);
    }
}