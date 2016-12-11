using System.IO;

namespace Petecat.Threading.Process
{
    public class ProcessObject
    {
        public ProcessObject(string executable)
        {
            Executable = executable;
        }

        public string Executable { get; private set; }

        public string Arguments { get; private set; }

        public ProcessObject Add(string argument)
        {
            Arguments += " " + argument;
            return this;
        }

        public string ReadString(string workingDirectory = null)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = Executable;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.Arguments = Arguments;
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.Start();

            using (var reader = process.StandardOutput)
            {
                return reader.ReadToEnd();
            }
        }

        public StreamReader ReadStream(string workingDirectory = null)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = Executable;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.Arguments = Arguments;
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.Start();

            return process.StandardOutput;
        }

        public void Execute(string workingDirectory = null)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = Executable;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Arguments = Arguments;
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.Start();
        }
    }
}