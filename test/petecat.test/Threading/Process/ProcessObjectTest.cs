using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petecat.Threading.Process;
using System;
using System.Collections.Generic;

namespace Petecat.Test.Threading.Process
{
    [TestClass]
    public class ProcessObjectTest
    {
        [TestMethod]
        public void ExecuteTest()
        {
            var changeFiles = new List<ChangeFile>();

            using (var reader = new ProcessObject("git")
                .Add("diff").Add("--name-status").Add("master..project_13400")
                .ReadStream(@"d:\git\itemservice"))
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
                .Add("log").Add("master..")
                .ReadStream(@"d:\git\itemservice"))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("commit", StringComparison.OrdinalIgnoreCase))
                    {
                        commits.Add(new Commit() { Id = line.Remove(0, 6).Trim() });
                    }
                }
            }

            //var initialVersion = new ProcessObject("git")
            //    .Add("show").Add("fcc8ae29481f7abf96a5d2cb8ae34ab7f5d58d88:Src/Item.Business/Implement/ItemBusiness.cs")
            //    .ReadString(@"d:\git\itemservice");

            //var latestVersion = new ProcessObject("git")
            //    .Add("show").Add("5464bd66a341ddb5878fd7f2e36672b84c369f90:Src/Item.Business/Implement/ItemBusiness.cs")
            //    .ReadString(@"d:\git\itemservice");

        }

        class ChangeFile
        {
            public string Action { get; set; }

            public string Path { get; set; }
        }

        class Commit
        {
            public string Id { get; set; }
        }
    }
}
