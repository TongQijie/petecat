using System;
using System.Linq;

namespace Achiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var terminalCommandLine = TerminalCommandLineUtility.Parse(string.Format("{0} {1}", "arch", string.Join(" ", args.Select(x => x.Contains(' ') ? ('\"' + x + '\"') : x))));
            if (terminalCommandLine.ContainKeys("m", "mode", "i", "input", "o", "output"))
            {
                Console.Write("mode/input/ouput argument is missing.");
                return;
            }

            if ((terminalCommandLine["m"] != null && terminalCommandLine["m"].Equals("ar", StringComparison.OrdinalIgnoreCase))
                || terminalCommandLine["mode"] != null && terminalCommandLine["mode"].Equals("ar", StringComparison.OrdinalIgnoreCase))
            {
                // 压缩文件
                try
                {
                    var archiver = new Archiver(terminalCommandLine["o"] ?? terminalCommandLine["ouput"], new string[] 
                    {
                        terminalCommandLine["i"] ?? terminalCommandLine["input"],
                    });
                    Console.Write("start archiving...");
                    archiver.Archive();
                    Console.Write("done.");
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    return;
                }
            }
            else if ((terminalCommandLine["m"] != null && terminalCommandLine["m"].Equals("un", StringComparison.OrdinalIgnoreCase))
                || terminalCommandLine["mode"] != null && terminalCommandLine["mode"].Equals("un", StringComparison.OrdinalIgnoreCase))
            {
                // 解压文件
                try
                {
                    var archiver = new Archiver(terminalCommandLine["o"] ?? terminalCommandLine["ouput"], terminalCommandLine["i"] ?? terminalCommandLine["input"]);
                    Console.Write("start unarchiving...");
                    archiver.Unarchive();
                    Console.Write("done.");
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    return;
                }
            }
            else
            {
                Console.Write("invalid mode.");
            }
        }
    }
}
