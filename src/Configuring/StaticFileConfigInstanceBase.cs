using System.IO;
using System.Text.RegularExpressions;

using Petecat.Utility;
using Petecat.Extending;
using Petecat.Configuring.Attribute;

namespace Petecat.Configuring
{
    public class StaticFileConfigInstanceBase : IStaticFileConfigInstance
    {
        public bool IsMultipleFiles { get; set; }

        public void Append(IStaticFileConfigurer configurer)
        {
            StaticFileConfigElementAttribute attribute;
            if (!Reflector.TryGetCustomAttribute(this.GetType(), null, out attribute))
            {
                // TODO: throw
            }

            IsMultipleFiles = attribute.IsMultipleFiles;

            if (IsMultipleFiles)
            {
                var path = attribute.Path.FullPath();

                var lastField = string.Empty;
                var idx = path.LastIndexOf('/');
                if (idx == -1)
                {
                    // TODO: throw
                }

                lastField = path.Substring(idx + 1);

                var directory = path.Substring(0, idx);
                if (!directory.IsFolder())
                {
                    // TODO: throw
                }

                var text = lastField.Replace("*", "\\S*");
                foreach (var fileInfo in new DirectoryInfo(directory).GetFiles())
                {
                    if (Regex.IsMatch(fileInfo.Name, text, RegexOptions.IgnoreCase))
                    {
                        configurer.Append(attribute.Key + "_" + fileInfo.Name, path, attribute.FileFormat, this.GetType());
                    }
                }
            }
            else
            {
                configurer.Append(attribute.Key, attribute.Path.FullPath(), attribute.FileFormat, this.GetType());
            }
        }
    }
}