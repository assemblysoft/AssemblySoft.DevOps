using System;

namespace AssemblySoft.DevOps
{

    public partial class TaskRunner
    {
        public class TaskStatusEventArg : EventArgs
        {
            public string Status { get; set; }
        }       

    }
}
