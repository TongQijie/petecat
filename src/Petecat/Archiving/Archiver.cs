using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

using Petecat.Utility;

namespace Petecat.Archiving
{
    public class Archiver
    {
        /// <summary>
        /// 归档文件
        /// </summary>
        public Archiver(string targetFile, string[] filesOrFoldersToBeArchived)
        {
            TargetPath = targetFile;

            string directoryPath = null;
            foreach (var fileOrFolder in filesOrFoldersToBeArchived)
            {
                var info = GetFileOrFolder(fileOrFolder);
                if (info == null)
                {
                    continue;
                }

                if (info is FileInfo)
                {
                    var fileInfo = info as FileInfo;
                    if (directoryPath != null)
                    {
                        if (directoryPath != fileInfo.DirectoryName)
                        {
                            throw new NotSupportedException();
                        }
                    }
                    else
                    {
                        directoryPath = fileInfo.DirectoryName;
                    }

                    var archiveFile = new ArchiveFile()
                    {
                        AbsolutePath = fileInfo.FullName,
                        RelativePath = fileInfo.Name,
                        Name = fileInfo.Name,
                        Length = fileInfo.Length,
                    };

                    ArchiveItems.Add(archiveFile);
                }
                else if (info is DirectoryInfo)
                {
                    var directoryInfo = info as DirectoryInfo;
                    if (directoryPath != null)
                    {
                        if (directoryPath != directoryInfo.Parent.FullName)
                        {
                            throw new NotSupportedException();
                        }
                    }
                    else
                    {
                        directoryPath = directoryInfo.Parent.FullName;
                    }

                    var archiveFolder = new ArchiveFolder()
                    {
                        AbsolutePath = directoryInfo.FullName,
                        RelativePath = directoryInfo.Name,
                        Name = directoryInfo.Name,
                    };

                    archiveFolder.GetArchiveItems();

                    ArchiveItems.Add(archiveFolder);
                }
            }
        }

        /// <summary>
        /// 解档文件
        /// </summary>
        public Archiver(string targetFolder, string sourceFile)
        {
            if (!File.Exists(sourceFile))
            {
                throw new FileNotFoundException();
            }

            TargetPath = targetFolder;
            SourcePath = sourceFile;
        }

        private List<IArchiveItem> _ArchiveItems = null;

        public List<IArchiveItem> ArchiveItems { get { return _ArchiveItems ?? (_ArchiveItems = new List<IArchiveItem>()); } }

        public string TargetPath { get; private set; }

        public string SourcePath { get; private set; }

        public void Archive()
        {
            using (var tempStream = new FileStream(TargetPath + ".tmp", FileMode.Create, FileAccess.ReadWrite))
            {
                var fileHeader = new ArchiveEntityHeader()
                {
                    RelativePath = "",
                    Length = ArchiveItems.Sum(x => x.GetFiles()),
                    HashValue = "",
                };
                fileHeader.WriteStream(tempStream);

                ArchiveItems.ForEach(x => x.WriteHeader(tempStream));
                ArchiveItems.ForEach(x => x.WriteContent(tempStream));

                tempStream.Seek(0, SeekOrigin.Begin);

                using (var outputStream = new FileStream(TargetPath, FileMode.Create, FileAccess.Write))
                {
                    CompressUtility.GzipCompress(tempStream, outputStream);
                }
            }

            File.Delete(TargetPath + ".tmp");
        }

        public void Unarchive()
        {
            using (var inputStream = new FileStream(SourcePath, FileMode.Open, FileAccess.Read))
            {
                using (var tempStream = new FileStream(SourcePath + ".tmp", FileMode.Create, FileAccess.ReadWrite))
                {
                    CompressUtility.GzipDecompress(inputStream, tempStream);

                    tempStream.Seek(0, SeekOrigin.Begin);

                    var header = new ArchiveEntityHeader();
                    header.ReadStream(tempStream);

                    for (int i = 0; i < header.Length; i++)
                    {
                        var archiveFile = new ArchiveFile();
                        archiveFile.ReadHeader(tempStream);
                        archiveFile.AbsolutePath = Path.Combine(TargetPath, archiveFile.RelativePath);

                        ArchiveItems.Add(archiveFile);
                    }

                    ArchiveItems.ForEach(x => x.ReadContent(tempStream));
                }

                File.Delete(SourcePath + ".tmp");
            }
        }

        private FileSystemInfo GetFileOrFolder(string path)
        {
            if (File.Exists(path))
            {
                return new FileInfo(path);
            }
            else if (Directory.Exists(path))
            {
                return new DirectoryInfo(path);
            }
            else
            {
                return null;
            }
        }
    }
}