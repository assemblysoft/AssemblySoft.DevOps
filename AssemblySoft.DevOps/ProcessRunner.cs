using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySoft.DevOps
{
    public class ProcessRunner
    {
        public string RunProcess(string processFilePath, string command = "", bool createNoWindow = true, bool useShellExecute = false)
        {
            var outputBuilder = new StringBuilder();

            var processInfo = new ProcessStartInfo(processFilePath, command)
            {
                CreateNoWindow = createNoWindow,
                UseShellExecute = useShellExecute,
            };

            var process = Process.Start(processInfo);

            if (process == null)
                return outputBuilder.ToString();


            process.WaitForExit();

            var exitCode = process.ExitCode;
            process.Close();

            return outputBuilder.ToString();
        }

        public string RunProcess(string processFilePath, string processFileName, string command, bool createNoWindow = true, bool useShellExecute = false)
        {
            var outputBuilder = new StringBuilder();

            var processInfo = new ProcessStartInfo(string.Format(@"{0}\{1}", processFilePath, processFileName), command)
            {
                CreateNoWindow = createNoWindow,
                UseShellExecute = useShellExecute,
            };

            var process = Process.Start(processInfo);

            if (process == null)
                return outputBuilder.ToString();


            process.WaitForExit();

            var exitCode = process.ExitCode;
            process.Close();

            return outputBuilder.ToString();
        }
    }
}
