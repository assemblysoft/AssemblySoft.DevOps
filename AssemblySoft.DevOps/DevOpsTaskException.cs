using System;

namespace AssemblySoft.DevOps
{
    public class DevOpsTaskException : Exception
    {
        public DevOpsTaskException()
        {

        }

        public DevOpsTaskException(string message):base(message)
        {

        }

        public DevOpsTaskException(string message, Exception inner) : base(message, inner)
        {

        }

        public DevOpsTaskException(DevOpsTask task, string message):base(message)
        {
            Task = task;
        }

        public DevOpsTaskException(DevOpsTask task, string message, Exception inner) : base(message, inner)
        {
            Task = task;
        }

        public DevOpsTask Task { get; private set; }
    }
}
