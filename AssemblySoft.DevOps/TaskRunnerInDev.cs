using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySoft.DevOps
{
    public partial class TaskRunner
    {
        #region review

        Assembly _invokingAssembly;

        public IEnumerable<DevOpsTask> GetDevOpsTaskWithState()
        {
            return _devOpsTasks;
        }   
        
        #endregion
    }
}
