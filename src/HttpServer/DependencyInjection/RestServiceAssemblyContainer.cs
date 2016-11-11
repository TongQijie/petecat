using Petecat.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Petecat.Extension;

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

            System.Diagnostics.Debug.Print(directoryInfo.FullName);

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
                        catch (Exception e)
                        {
                            // TODO: throw
                        }

                        System.Diagnostics.Debug.Print("register dll: " + fileInfo.FullName);
                    }
                }
            }
        }

        private DirectoryInfo GetRootAssemblyDirectory(string currentAssemblyPath)
        {
            var directoryInfo = new FileInfo(currentAssemblyPath).Directory;
            while (directoryInfo.Name != "dl3")
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
                // TODO: throw
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
                // TODO: throw
            }

            var obj = DependencyInjector.GetObject(typeDefinition.Info as Type);
            if (obj == null)
            {
                // TODO: throw
            }

            if (request.Request.HttpMethod == "GET")
            {
                if (methodInfo.ParameterInfos == null || methodInfo.ParameterInfos.Length == 0)
                {
                    return methodInfo.Invoke(obj, null);
                }
                else
                {
                    var queryStringValues = request.ReadQueryString().Values;

                    var values = new string[methodInfo.ParameterInfos.Length];

                    for (var i = 0; i < values.Length; i++)
                    {
                        var parameterInfo = methodInfo.ParameterInfos.FirstOrDefault(x => x.Index == i);

                        var value = queryStringValues.FirstOrDefault(x => string.Equals(x, parameterInfo.ParameterName, StringComparison.OrdinalIgnoreCase));
                        if (value == null)
                        {
                            // TODO: throw
                        }

                        values[i] = value;
                    }

                    return methodInfo.Invoke(obj, values);
                }
            }
            else
            {
                if (methodInfo.ParameterInfos == null || methodInfo.ParameterInfos.Length != 1)
                {
                    // TODO: throw
                }

                return methodInfo.Invoke(obj, request.ReadInputStream(methodInfo.ParameterInfos[0].TypeDefinition.Info as Type));
            }
        }
    }
}