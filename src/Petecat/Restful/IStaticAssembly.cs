using System.Reflection;
namespace Petecat.Restful
{
    /// <summary>
    /// Dotnet static assembly function interface.
    /// </summary>
    public interface IStaticAssembly
    {
        /// <summary>
        /// Loads the contents of an assembly file on the specified path.
        /// </summary>
        /// <param name="path">The path of the file to load.</param>
        /// <returns>The loaded assembly.</returns>
        /// <exception cref="T:System.ArgumentNullException">The path parameter is null.</exception>
        /// <exception cref="T:System.IO.FileLoadException">A file that was found could not be loaded.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The path parameter is an empty string ("") or does not exist.</exception>
        /// <exception cref="T:System.BadImageFormatException">path is not a valid assembly. -or-Version 2.0 or later of the common language runtime is currently loaded and path was compiled with a later version.</exception>
        Assembly LoadFile(string path);
    }
}
