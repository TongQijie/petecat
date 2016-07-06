using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
namespace Petecat.Restful
{
    /// <summary>
    /// Default assembly type loader.
    /// </summary>
    [AutoSetupService(typeof(IAssemblyTypeLoader))]
    internal class DefaultAssemblyTypeLoader : IAssemblyTypeLoader
    {
        /// <summary>
        /// Get all types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <returns>Type collection.</returns>
        public IEnumerable<Type> GetTypes(Assembly assembly)
        {
            string message;
            return this.GetTypes(assembly, out message);
        }

        /// <summary>
        /// Get all types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <param name="message">Process message.</param>
        /// <returns>Type collection.</returns>
        public IEnumerable<Type> GetTypes(Assembly assembly, out string message)
        {
            IEnumerable<Type> result = Enumerable.Empty<Type>();
            message = string.Empty;
            try
            {
                result = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder builder = new StringBuilder();
                if (!ex.Types.IsNullOrEmpty<Type>())
                {
                    (from type in ex.Types
                     where type != null
                     select type).ForEach(delegate(Type type)
                     {
                         builder.AppendFormat("Load type: \"{0}\" fail. ", type.FullName);
                     });
                }
                if (!ex.LoaderExceptions.IsNullOrEmpty<Exception>())
                {
                    (from x in ex.LoaderExceptions
                     where x != null
                     select x).ForEach(delegate(Exception x)
                     {
                         builder.AppendFormat("Load exception: \"{0}\". ", x.Message);
                     });
                }
                message = builder.ToString();
            }
            catch (Exception ex2)
            {
                message = ex2.Message;
            }
            return result;
        }

        /// <summary>
        /// Get filted types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <param name="filter">Filter function.</param>
        /// <returns>Type collection.</returns>
        public IEnumerable<Type> GetTypes(Assembly assembly, Func<Type, bool> filter)
        {
            string message;
            return this.GetTypes(assembly, filter, out message);
        }

        /// <summary>
        /// Get filted types from assemblies.
        /// </summary>
        /// <param name="assembly">Assembly instance.</param>
        /// <param name="filter">Filter function.</param>
        /// <param name="message">Process message.</param>
        /// <returns>Type collection.</returns>
        public IEnumerable<Type> GetTypes(Assembly assembly, Func<Type, bool> filter, out string message)
        {
            IEnumerable<Type> result = Enumerable.Empty<Type>();
            message = string.Empty;
            try
            {
                result = (from type in assembly.GetTypes()
                          where filter(type)
                          select type).ToArray<Type>();
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder builder = new StringBuilder();
                if (!ex.Types.IsNullOrEmpty<Type>())
                {
                    (from type in ex.Types
                     where type != null
                     select type).ForEach(delegate(Type type)
                     {
                         builder.AppendFormat("Load type: \"{0}\" fail. ", type.FullName);
                     });
                }
                if (!ex.LoaderExceptions.IsNullOrEmpty<Exception>())
                {
                    (from x in ex.LoaderExceptions
                     where x != null
                     select x).ForEach(delegate(Exception x)
                     {
                         builder.AppendFormat("Load exception: \"{0}\". ", x.Message);
                     });
                }
                message = builder.ToString();
            }
            catch (Exception ex2)
            {
                message = ex2.Message;
            }
            return result;
        }
    }
}
