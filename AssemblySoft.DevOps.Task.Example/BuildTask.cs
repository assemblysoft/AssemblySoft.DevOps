using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySoft.DevOps.Task.Example
{
    public class BuildTask
    {
        public async Task<DevOpsTaskStatus> Build()
        {

            var procRunner = new ProcessRunner.ProcessRunner();
            var result = procRunner.RunProcess(ConfigurationManager.AppSettings["buildProc"], "t_deploy.bat","18.1.1.9",useShellExecute:false,createNoWindow:false);
            return DevOpsTaskStatus.Completed;
        }
    }
}
