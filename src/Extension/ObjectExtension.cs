using Petecat.Formatter;
using Petecat.DependencyInjection;

using System.Text;

namespace Petecat.Extension
{
    public static class ObjectExtension
    {
        public static T Copy<T>(this T obj)
        {
            var formatter = DependencyInjector.GetObject<IJsonFormatter>();
            return formatter.ReadObject<T>(formatter.WriteString(obj, Encoding.UTF8), Encoding.UTF8);
        }
    }
}