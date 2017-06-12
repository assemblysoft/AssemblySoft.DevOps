using System;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AssemblySoft.DevOps
{
    public class DevOpsTask
    {
        public string Id { get; set; }
        
        public string Assembly { get; set; }
        public string Namespace { get; set; }
        public string Method { get; set; }
        public bool Enabled { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }        
        public DevOpsTaskStatus Status;
                
        [XmlIgnore]
        public string LastExecutionDuration { get; set; }
        
        public delegate void ActionRef<DevOpsTaskStatus>(ref DevOpsTaskStatus item);

        [NonSerialized]
        [XmlIgnore]
        public Func<DevOpsTaskStatus, Task> runTask;

        [NonSerialized]
        [XmlIgnore]
        public Func<Task<DevOpsTaskStatus>> func;

        [NonSerialized]
        [XmlIgnore]
        public ActionRef<DevOpsTaskStatus> runActionRef;

        [NonSerialized]
        [XmlIgnore]
        public Action<DevOpsTaskStatus> runActionVal;

        [NonSerialized]
        [XmlIgnore]
        public Action runAction;      

        public async Task Run(DevOpsTaskStatus status)
        {
            if(runTask == null)
            {

            }

            await runTask(status); 
                       
        }       

        public void Run()
        {
            runAction?.Invoke();
        }
    }
}
