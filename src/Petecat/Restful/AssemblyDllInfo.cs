using System;

namespace Petecat.Restful
{
    /// <summary>
    /// Assembly dll file info.
    /// </summary>
    public class AssemblyDllInfo
    {
        /// <summary>
        /// Gets or sets file name.
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets full path of file.
        /// </summary>
        public string FullPath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets last edited time of file.
        /// </summary>
        public DateTime LastTime
        {
            get;
            set;
        }
    }
}
