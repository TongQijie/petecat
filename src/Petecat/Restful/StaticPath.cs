using System.IO;

namespace Petecat.Restful
{
    /// <summary>
    /// Static path.
    /// </summary>
    [AutoSetupService(typeof(IStaticPath))]
    internal class StaticPath : IStaticPath
    {
        /// <summary>
        /// Combines an array of strings into a path.
        /// </summary>
        /// <param name="paths">An array of parts of the path.</param>
        /// <returns>The combined paths.</returns>
        /// <exception cref="T:System.ArgumentException">One of the strings in the array contains one or more of the invalid characters defined in System.IO.Path.GetInvalidPathChars().</exception>
        /// <exception cref="T:System.ArgumentNullException">One of the strings in the array is null.</exception>
        public string Combine(params string[] paths)
        {
            return Path.Combine(paths);
        }

        /// <summary>
        /// Returns the directory information for the specified path string.
        /// </summary>
        /// <param name="path">The path of a file or directory.</param>
        /// <returns>Directory information for path, or null if path denotes a root directory or is null. Returns System.String.Empty if path does not contain directory information.</returns>
        /// <exception cref="T:System.ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
        /// <exception cref="T:System.IO.PathTooLongException">The path parameter is longer than the system-defined maximum length.</exception>
        public string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Returns the extension of the specified path string.
        /// </summary>
        /// <param name="path">The path string from which to get the extension.</param>
        /// <returns>The extension of the specified path (including the period "."), or null, or System.String.Empty. If path is null, System.IO.Path.GetExtension(System.String) returns null. If path does not have extension information, System.IO.Path.GetExtension(System.String) returns System.String.Empty.</returns>
        /// <exception cref="T:System.ArgumentException">Path contains one or more of the invalid characters defined in System.IO.Path.GetInvalidPathChars().</exception>
        public string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        /// <summary>
        /// Returns the file name and extension of the specified path string.
        /// </summary>
        /// <param name="path">The path string from which to obtain the file name and extension.</param>
        /// <returns>The characters after the last directory character in path. If the last character of path is a directory or volume separator character, this method returns System.String.Empty. If path is null, this method returns null.</returns>
        /// <exception cref="T:System.ArgumentException">Path contains one or more of the invalid characters defined in System.IO.Path.GetInvalidPathChars().</exception>
        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// Returns the file name of the specified path string without the extension.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        /// <returns>The string returned by System.IO.Path.GetFileName(System.String), minus the last period (.) and all characters following it.</returns>
        /// <exception cref="T:System.ArgumentException">Path contains one or more of the invalid characters defined in System.IO.Path.GetInvalidPathChars().</exception>
        public string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// Returns the absolute path for the specified path string.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain absolute path information.</param>
        /// <returns>The fully qualified location of path, such as "C:\MyFile.txt".</returns>
        /// <exception cref="T:System.ArgumentException">Path is a zero-length string, contains only white space, or contains one or more of the invalid characters defined in System.IO.Path.GetInvalidPathChars().-or- The system could not retrieve the absolute path.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions.</exception>
        /// <exception cref="T:System.ArgumentNullException">Path is null.</exception>
        /// <exception cref="T:System.NotSupportedException">Path contains a colon (":") that is not part of a volume identifier (for example, "c:\").</exception>
        /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        public string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        /// <summary>
        /// Determines whether a path includes a file name extension.
        /// </summary>
        /// <param name="path">The path to search for an extension.</param>
        /// <returns>True if the characters that follow the last directory separator (\\ or /) or volume separator (:) in the path include a period (.) followed by one or more characters; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentException">Path contains one or more of the invalid characters defined in System.IO.Path.GetInvalidPathChars().</exception>
        public bool HasExtension(string path)
        {
            return Path.HasExtension(path);
        }

        /// <summary>
        /// Gets a value indicating whether the specified path string contains a root.
        /// </summary>
        /// <param name="path">The path to test.</param>
        /// <returns>True if path contains a root; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentException">Path contains one or more of the invalid characters defined in System.IO.Path.GetInvalidPathChars().</exception>
        public bool IsPathRooted(string path)
        {
            return Path.IsPathRooted(path);
        }
    }
}
