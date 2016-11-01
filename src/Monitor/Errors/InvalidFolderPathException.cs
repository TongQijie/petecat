using System;

namespace Petecat.Monitor.Errors
{
    public class InvalidFolderPathException : Exception
    {
        public InvalidFolderPathException(string path)
            : base(string.Format("'{0}' is invalid folder path.", path))
        {
        }
    }
}
