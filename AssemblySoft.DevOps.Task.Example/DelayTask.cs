using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssemblySoft.DevOps.Task.Example
{
    public class DelayTask
    {
        public DelayTask()
        {
        }

        public async Task<DevOpsTaskStatus> GoToSleepForTenSeconds()
        {            
            //await Task.Delay(10000);
            Thread.Sleep(10000);

            return DevOpsTaskStatus.Completed;
        }
    }
}
