using System;

namespace AssemblySoft.DevOps.Common
{

    /// <summary>
    /// Base class for tasks that require notifications 
    /// </summary>
    public abstract class NotifyTask
    {
        public event EventHandler<NotifyTaskOutputEventArgs> NotifyTaskOutputData;
                
        public void RaiseOutputEvent(string message)
        {
            NotifyTaskOutputData?.Invoke(this, new NotifyTaskOutputEventArgs() { Message = message });
        }
    }

    public class NotifyTaskOutputEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
