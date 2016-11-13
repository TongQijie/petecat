using Petecat.Utility;
using Petecat.Extension;
using Petecat.Configuring.Attribute;

namespace Petecat.Configuring
{
    public class StaticFileConfigInstanceBase : IStaticFileConfigInstance
    {
        public void Append(IStaticFileConfigurer configurer)
        {
            StaticFileConfigElementAttribute attribute;
            if (!Reflector.TryGetCustomAttribute(this.GetType(), null, out attribute))
            {
                // TODO: throw
            }

            configurer.Append(attribute.Key, attribute.Path.FullPath(), attribute.FileFormat, this.GetType());
        }
    }
}