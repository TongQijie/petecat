using Petecat.Console;
using Petecat.Threading.Process;
using System.Collections.Generic;
using System.IO;

namespace GitDiff
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length != 3)
            {
                ConsoleBridging.WriteLine("args error: gitdiff [BranchName] [LocalRepo] [CompareTool]");
                return;
            }

            string branchName = args[0];

            string localLocation = args[1];

            string compareTool = args[2];

            var changeFiles = new List<ChangeFile>();

            using (var reader = new ProcessObject("git")
                .Add("diff").Add("--name-status").Add("master.." + branchName)
                .ReadStream(localLocation))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split('\t');
                    if (fields.Length == 2)
                    {
                        changeFiles.Add(new ChangeFile() { Action = fields[0].Trim(), Path = fields[1].Trim() });
                    }
                }
            }

            var commits = new List<Commit>();

            using (var reader = new ProcessObject("git")
                .Add("reflog").Add("show")
                .ReadStream(localLocation))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    commits.Add(new Commit() { Id = line.Trim().Substring(0, 7), Text = line.Trim() });
                }
            }

            if (commits.Count <= 1)
            {
                return;
            }

            var initialVersion = commits.FindLast(x => x.Text.ToUpper().Contains(branchName.ToUpper()));
            var latestVersion = commits[0];

            foreach (var changeFile in changeFiles)
            {
                var initialContent = new ProcessObject("git")
                    .Add("show").Add(initialVersion.Id + ":" + changeFile.Path)
                    .ReadString(localLocation);

                changeFile.InitialFullPath = Path.Combine("init", changeFile.Path.Replace('/', '\\'));
                WriteFile(initialContent, changeFile.InitialFullPath);

                var latestContent = new ProcessObject("git")
                    .Add("show").Add(latestVersion.Id + ":" + changeFile.Path)
                    .ReadString(localLocation);

                changeFile.LatestFullPath = Path.Combine("latest", changeFile.Path.Replace('/', '\\'));
                WriteFile(latestContent, changeFile.LatestFullPath);
            }

            for (var i = 1; i <= changeFiles.Count; i++)
            {
                ConsoleBridging.WriteLine("{0,-5}{1}", i, changeFiles[i - 1].Path);
            }

            var command = "";
            while ((command = ConsoleBridging.ReadLine()) != "quit")
            {
                if (command.Trim().Equals("list", System.StringComparison.OrdinalIgnoreCase))
                {
                    for (var i = 1; i <= changeFiles.Count; i++)
                    {
                        ConsoleBridging.WriteLine("{0,-5}{1}", i, changeFiles[i - 1].Path);
                    }
                }
                else
                {
                    int index;
                    if (!int.TryParse(command.Trim(), out index) || index <= 0 || index > changeFiles.Count)
                    {
                        ConsoleBridging.WriteLine("error index.");
                    }
                    else
                    {
                        new ProcessObject(compareTool)
                            .Add(changeFiles[index - 1].InitialFullPath).Add(changeFiles[index - 1].LatestFullPath)
                            .Execute();
                    }
                }
            }
        }

        private static void WriteFile(string content, string path)
        {
            var folder = path.Substring(0, path.LastIndexOf('\\'));
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            using (var outputStream = new StreamWriter(path))
            {
                outputStream.Write(content);
            }
        }

        class ChangeFile
        {
            public string Action { get; set; }

            public string Path { get; set; }

            public string InitialFullPath { get; set; }

            public string LatestFullPath { get; set; }

        }

        class Commit
        {
            public string Id { get; set; }

            public string Text { get; set; }
        }
    }
}
