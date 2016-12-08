using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Achiver
{
    public class ArchiveFolder : AbstractArchiveItem
    {
        private List<IArchiveItem> _ArchiveItems = null;

        public List<IArchiveItem> ArchiveItems { get { return _ArchiveItems ?? (_ArchiveItems = new List<IArchiveItem>()); } }

        public override int GetFiles()
        {
            return ArchiveItems.Sum(x => x.GetFiles());
        }

        public void GetArchiveItems()
        {
            var currentDirectoryInfo = new DirectoryInfo(AbsolutePath);

            foreach (var fileInfo in currentDirectoryInfo.GetFiles())
            {
                var archiveFile = new ArchiveFile()
                {
                    AbsolutePath = fileInfo.FullName,
                    RelativePath = RelativePath + @"\" + fileInfo.Name,
                    Name = fileInfo.Name,
                    Length = fileInfo.Length,
                };

                ArchiveItems.Add(archiveFile);
            }

            foreach (var directoryInfo in currentDirectoryInfo.GetDirectories())
            {
                var archiveFolder = new ArchiveFolder()
                {
                    AbsolutePath = directoryInfo.FullName,
                    RelativePath = RelativePath + @"\" + directoryInfo.Name,
                    Name = directoryInfo.Name,
                };

                archiveFolder.GetArchiveItems();

                ArchiveItems.Add(archiveFolder);
            }
        }

        public override void WriteHeader(Stream outputStream)
        {
            ArchiveItems.ForEach(x => x.WriteHeader(outputStream));
        }

        public override void WriteContent(Stream outputStream)
        {
            ArchiveItems.ForEach(x => x.WriteContent(outputStream));
        }
    }
}
