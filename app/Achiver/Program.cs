using Petecat.Console;
using Petecat.Console.Command;
using Petecat.Archiving;

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
                CommonUtility.Write("mode/input/ouput argument is missing.");
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
                    CommonUtility.Write("start archiving...");
                    archiver.Archive();
                    CommonUtility.Write("done.");
                }
                catch (Exception e)
                {
                    CommonUtility.Write(e.Message);
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
                    CommonUtility.Write("start unarchiving...");
                    archiver.Unarchive();
                    CommonUtility.Write("done.");
                }
                catch (Exception e)
                {
                    CommonUtility.Write(e.Message);
                    return;
                }
            }
            else
            {
                CommonUtility.Write("invalid mode.");
            }
        }
    }
}
