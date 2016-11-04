using System;

namespace Petecat.Configuring
{
    public interface IStaticFileConfigurer
    {
        /// <summary>
        /// adds static file in file system to cache as configuration source.
        /// </summary>
        /// <param name="key">configuration key</param>
        /// <param name="path">static file path</param>
        /// <param name="fileFormat">static file format, supporting 'xml' and 'json'</param>
        /// <param name="configurationType">type of configuration value</param>
        void Append(string key, string path, string fileFormat, Type configurationType);

        /// <summary>
        /// removes configuration source from cache
        /// </summary>
        /// <param name="key">configuration key</param>
        void Remove(string key);

        /// <summary>
        /// gets configuration value
        /// </summary>
        /// <param name="key">configuration key</param>
        /// <returns>configuration value</returns>
        object GetValue(string key);

        /// <summary>
        /// gets configuration value
        /// </summary>
        /// <typeparam name="T">type of configuration value</typeparam>
        /// <param name="key">configuration key</param>
        /// <returns>configuration value</returns>
        T GetValue<T>(string key);
    }
}
