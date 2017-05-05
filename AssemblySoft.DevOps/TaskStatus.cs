using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblySoft.DevOps
{
    /// <summary>
    /// The task status.
    /// </summary>
    public enum DevOpsTaskStatus
    {
        /// <summary>
        /// The Idle status.
        /// </summary>
        Idle,

        /// <summary>
        /// The started status.
        /// </summary>
        Started,

        /// <summary>
        /// The running status.
        /// </summary>
        Running,

        /// <summary>
        /// The skipped status.
        /// </summary>
        Skipped,

        /// <summary>
        /// The blocked status.
        /// </summary>
        Blocked,

        /// <summary>
        /// The faulted status.
        /// </summary>
        Faulted,

        /// <summary>
        /// The paused status.
        /// </summary>
        Paused,

        /// <summary>
        /// The terminated status
        /// </summary>
        /// <remarks>
        /// prematurely ended 
        /// </remarks>
        Terminated,

        /// <summary>
        /// The completed status.
        /// </summary>
        Completed
    }
}
