using System;
using System.IO;

namespace Petecat.Archiving
{
    public abstract class AbstractArchiveItem : IArchiveItem
    {
        public string AbsolutePath { get; set; }

        public string RelativePath { get; set; }

        public string Name { get; set; }

        public string GetFolder()
        {
            var index = AbsolutePath.LastIndexOf('\\');
            if (index == -1)
            {
                return null;
            }

            return AbsolutePath.Remove(index, AbsolutePath.Length - index);
        }

        public virtual void WriteHeader(Stream outputStream)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteContent(Stream outputStream)
        {
            throw new NotImplementedException();
        }

        public virtual void ReadHeader(Stream inputStream)
        {
            throw new NotImplementedException();
        }

        public virtual void ReadContent(Stream inputStream)
        {
            throw new NotImplementedException();
        }

        public virtual int GetFiles()
        {
            throw new NotImplementedException();
        }
    }
}
