using System;
namespace Petecat.Configuration
{
    [Obsolete("replaced by CacheObjectManager")]
    public interface IFileConfigurationManager : IConfigurationManager
    {
        void LoadFromXml<T>(string filename, string key) where T : class;

        void LoadFromIni<T>(string filename, string key);

        void LoadFromJson<T>(string filename, string key) where T : class;
    }
}