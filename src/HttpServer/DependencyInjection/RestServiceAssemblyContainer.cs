using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Petecat.Extending;
using Petecat.DependencyInjection;

namespace Petecat.HttpServer.DependencyInjection
{
    public class RestServiceAssemblyContainer : AssemblyContainerBase
    {
        public RestServiceAssemblyContainer()
        {
            RegisterAssemblies();
        }

        private void RegisterAssemblies()
        {
            var directoryInfo = GetRootAssemblyDirectory(Assembly.GetExecutingAssembly().Location);

            foreach (var df in directoryInfo.GetDirectories())
            {
                var i = df.GetDirectories().OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
                if (i != null)
                {
                    foreach (var fileInfo in i.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
                    {
                        try
                        {
                            RegisterAssembly(new RestServiceAssemblyInfo(Assembly.LoadFile(fileInfo.FullName)));
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }

        private DirectoryInfo GetRootAssemblyDirectory(string currentAssemblyPath)
        {
            var directoryInfo = new FileInfo(currentAssemblyPath).Directory;
            // ASP.NET temperary folder.
            // iis: dl3
            // xsp: shadow
            while (directoryInfo.Name != "dl3" && directoryInfo.Name != "shadow")
            {
                directoryInfo = directoryInfo.Parent;
            }

            return directoryInfo;
        }

        public object Execute(RestServiceHttpRequest request)
        {
            var typeDefinition = RegisteredTypes.Values.OfType<RestServiceTypeDefinition>()
                .FirstOrDefault(x => string.Equals(x.ServiceName, request.ServiceName, StringComparison.OrdinalIgnoreCase));
            if (typeDefinition == null)
            {
                throw new Exception(string.Format("service '{0}' cannot be found.", request.ServiceName));
            }

            RestServiceInstanceMethodInfo methodInfo = null;
            if (request.MethodName.HasValue())
            {
                methodInfo = typeDefinition.InstanceMethods.OfType<RestServiceInstanceMethodInfo>()
                    .FirstOrDefault(x => string.Equals(x.ServiceMethodName, request.MethodName, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                methodInfo = typeDefinition.InstanceMethods.OfType<RestServiceInstanceMethodInfo>()
                    .FirstOrDefault(x => x.IsDefaultMethod);
            }
            if (methodInfo == null)
            {
                throw new Exception(string.Format("method '{0}' cannot be found.", request.MethodName));
            }

            var obj = DependencyInjector.GetObject(typeDefinition.Info as Type);
            if (obj == null)
            {
                throw new Exception(string.Format("failed to create object '{0}'.", (typeDefinition.Info as Type).FullName));
            }

            if (request.Request.HttpMethod == "GET")
            {
                if (methodInfo.ParameterInfos == null || methodInfo.ParameterInfos.Length == 0)
                {
                    return methodInfo.Invoke(obj, null);
                }
                else
                {
                    var dict = request.ReadQueryString();

                    var values = new string[methodInfo.ParameterInfos.Length];

                    for (var i = 0; i < values.Length; i++)
                    {
                        var parameterInfo = methodInfo.ParameterInfos.FirstOrDefault(x => x.Index == i);

                        if (!dict.Keys.ToArray().Exists(x => string.Equals(x, parameterInfo.ParameterName, StringComparison.OrdinalIgnoreCase)))
                        {
                            throw new Exception(string.Format("parameter '{0}' does not exist.", parameterInfo.ParameterName));
                        }

                        values[i] = dict.FirstOrDefault(x => string.Equals(x.Key, parameterInfo.ParameterName, StringComparison.OrdinalIgnoreCase)).Value;
                    }

                    return methodInfo.Invoke(obj, values);
                }
            }
            else
            {
                if (methodInfo.ParameterInfos == null || methodInfo.ParameterInfos.Length != 1)
                {
                    throw new Exception(string.Format("method '{0}' must have one parameter.", methodInfo.MethodName));
                }

                return methodInfo.Invoke(obj, request.ReadInputStream(methodInfo.ParameterInfos[0].TypeDefinition.Info as Type, methodInfo.DataFormat));
            }
        }
    }
}