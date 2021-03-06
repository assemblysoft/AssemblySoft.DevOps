using AssemblySoft.DevOps.Common;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace AssemblySoft.DevOps.Task.Example
{
    /// <summary>
    /// Example build task
    /// </summary>
    /// <remarks>
    /// Derives from NotifyTask to emit output to the task runner
    /// </remarks>
    public class BuildTask : NotifyTask
    {       

        /// <summary>
        /// Example method implementation
        /// </summary>
        /// <returns>Task status, used to notify the task runner of a success or failure state</returns>
        /// <remarks>
        /// Uses the abstract base class to send notifications back to the client/s
        /// </remarks>
        public async Task<DevOpsTaskStatus> RunBuildScript(string rootPath)
        {
            const string buildScript = "build.bat";
            var args = "3.1.8";

            var procRunner = new ProcessRunner.ProcessRunner(); //the thing we are consuming emits some useful information
            procRunner.ProcessOutputData += (s,e) => //subscribe to it's notification event
            {
                RaiseOutputEvent(e.Message); //pass that onto the task runner
            };

            try
            {
                //kick off the work
                var result = procRunner.RunProcess(Path.Combine(rootPath,Constants.SCRIPTS), buildScript, args, useShellExecute: false, createNoWindow: false);
                                
                #region Test Raise Exception
                //used to test throwing an exception from within a custom task 
                
                //throw new InvalidOperationException("ooops!");

                #endregion
            }
            catch(Exception e)
            {
                RaiseOutputEvent(e.Message);
                return DevOpsTaskStatus.Faulted;
            }

            
            return DevOpsTaskStatus.Completed;
        }

    }
    
}
