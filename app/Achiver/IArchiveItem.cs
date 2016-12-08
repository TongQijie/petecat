using System.IO;

namespace Achiver
{
    public interface IArchiveItem
    {
        string AbsolutePath { get; }

        string RelativePath { get; }

        string Name { get; }

        string GetFolder();

        int GetFiles();

        void WriteHeader(Stream outputStream);

        void WriteContent(Stream outputStream);

        void ReadHeader(Stream inputStream);

        void ReadContent(Stream inputStream);
    }
}
