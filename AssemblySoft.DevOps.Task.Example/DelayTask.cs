using System.Threading;
using System.Threading.Tasks;

namespace AssemblySoft.DevOps.Task.Example
{
    public class DelayTask
    {
        public DelayTask()
        {
        }

        /// <summary>
        /// Sleep for ten seconds
        /// </summary>
        /// <returns></returns>
        public async Task<DevOpsTaskStatus> GoToSleepForTenSeconds()
        {            
            //await Task.Delay(10000);
            Thread.Sleep(10000);

            return DevOpsTaskStatus.Completed;
        }

        /// <summary>
        /// Useful for simulating long running task, particularly around thread pool timeouts in a web context
        /// </summary>
        /// <returns></returns>
        public async Task<DevOpsTaskStatus> GoToSleepForTenMinutes()
        {            
            Thread.Sleep(600000); //600,000 milliseconds

            return DevOpsTaskStatus.Completed;
        }
    }
}
