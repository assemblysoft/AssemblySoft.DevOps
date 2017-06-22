using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using AssemblySoft.Serialization;
using AssemblySoft.DevOps.Common;

namespace AssemblySoft.DevOps
{
    /// <summary>
    /// Rsponsible for running a series of tasks
    /// </summary>
    public partial class TaskRunner
    {
        public TaskRunner()
        {
        }
        
        IEnumerable<DevOpsTask> _devOpsTasks;

        /// <summary>
        /// Run collection of Dev Ops Tasks given a file path
        /// </summary>
        /// <param name="filePath">File path to load task collection from</param>
        public void Run(string filePath)
        {
            ICollection<DevOpsTask> tasks;
            try
            {
                tasks = LoadTasksFromFile(filePath);

                if (tasks == null || tasks.Count <= 0)
                {
                    throw new DevOpsTaskException("Unable to load tasks from file path");
                }
            }
            catch (Exception ex)
            {
                throw new DevOpsTaskException("Unable to load tasks from file path", ex);
            }

            Run(tasks);
        }

        /// <summary>
        /// Run collection of Dev Ops Tasks
        /// </summary>
        /// <param name="devOpsTasks"></param>
        public void Run(IEnumerable<DevOpsTask> devOpsTasks)
        {
            string result = "undefined";
            try
            {
                var funcCount = BindFuncs(devOpsTasks.Where(t => t.Enabled == true)); //filter out tasks marked as disabled
                if (funcCount <= 0)
                {
                    throw new DevOpsTaskException("Unable to link methods to tasks");
                }
            }
            catch (Exception ex)
            {
                throw new DevOpsTaskException("Unable to bind methods to task declarations", ex);
            }

            _devOpsTasks = devOpsTasks.ToList();
            DevOpsTask currentDevOpsTask;
            int currentOrder = 0;

            //split into groups by order
            List<List<DevOpsTask>> orderGrouped = new List<List<DevOpsTask>>();
            var devOpsTaskGroups = _devOpsTasks.GroupBy(t => t.Order);

            string correlationId = Guid.NewGuid().ToString();
            BroadcastStatus(String.Format("Started processing tasks {0} {1}", correlationId, DateTime.UtcNow));

            foreach (var devOpsTaskSet in devOpsTaskGroups)
            {
                List<Task> tasksCollection = new List<Task>();

                foreach (var devOpsTask in devOpsTaskSet)
                {
                    currentDevOpsTask = devOpsTask;
                    currentOrder = devOpsTask.Order;

                    try
                    {
                        devOpsTask.Status = DevOpsTaskStatus.Started;

                        if (!devOpsTask.Enabled)
                        {
                            devOpsTask.Status = DevOpsTaskStatus.Skipped;
                        }

                        BroadcastStatus(String.Format("{0} {1} {2} ({3}) {4}", devOpsTask.Status, devOpsTask.Description, correlationId, devOpsTask.Order, DateTime.UtcNow));

                        List<Task> tasks = new List<Task>();
                        if (devOpsTask.Enabled)
                        {                            
                            Task task = new Task(() =>
                            {
                                BroadcastStatus(String.Format("Task Id: {0} Thread Id {1} {2} ({3}) {4}", Task.CurrentId, Thread.CurrentThread.ManagedThreadId, correlationId, devOpsTask.Order, devOpsTask.Description));

                                var start = System.Environment.TickCount;

                                if (devOpsTask.func != null)
                                {
                                    devOpsTask.Status = devOpsTask.func.Invoke().Result;
                                }

                                var stop = System.Environment.TickCount;
                                double elapsedTimeSeconds = (stop - start) / 1000;
                                devOpsTask.LastExecutionDuration = string.Format("{0:#,##0.00}", elapsedTimeSeconds);

                                if (devOpsTask.Enabled) //only report on enabled tasks                    
                                {
                                    BroadcastStatus(String.Format("{0} {1} {2} ({3}) {4} {5}", devOpsTask.Status, devOpsTask.Description, correlationId, devOpsTask.Order, devOpsTask.LastExecutionDuration, DateTime.UtcNow));
                                    //task.Enabled = false; //updates to disabled
                                }

                            });

                            tasksCollection.Add(task);
                            task.Start();
                        }
                    }
                    catch (Exception ex)
                    {
                        //ToDo: change method of logging as this may also cause an exception
                        throw new DevOpsTaskException(currentDevOpsTask, ex.Message);
                    }
                    finally
                    {
                    }
                }


                Task t = Task.WhenAll(tasksCollection);
                try
                {
                    t.Wait();
                }
                catch
                {

                }

                result = t.Status.ToString();

                if (t.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    BroadcastStatus(String.Format("All tasks in the set completed {0} ({1})", correlationId, currentOrder));
                }
                else
                {
                    BroadcastStatus(String.Format("At least one task failed to run with status: {0} {1}", correlationId, t.Status));

                    //show report of all tasks for brevity
                    foreach (var task in devOpsTaskSet)
                    {
                        if (task.Enabled) //only report on enabled tasks                    
                        {
                            BroadcastStatus(String.Format("{0} {1} {2} ({3}) {4}", task.Status, task.Description, correlationId, task.Order, task.LastExecutionDuration));
                        }
                    }
                }

            }

            BroadcastStatus(String.Format("Stopped processing tasks {0}", DateTime.UtcNow));

            RaiseTasksCompletedEvent(result);
        }

        /// <summary>
        /// Broadcasts the current status of a task
        /// </summary>
        /// <param name="msg"></param>
        private void BroadcastStatus(string msg)
        {
            RaiseTaskStatusEvent(msg);
        }

        public delegate void TaskStatusEventHandler(TaskStatusEventArg e);
        public event TaskStatusEventHandler TaskStatus;
        /// <summary>
        /// Raises a task status event
        /// </summary>
        /// <param name="status"></param>
        private void RaiseTaskStatusEvent(string status)
        {
            TaskStatusEventArg statusArgs = new TaskStatusEventArg
            {
                Status = status
            };

            TaskStatus?.Invoke(statusArgs);
        }

        public event TaskStatusEventHandler TasksCompleted;
        /// <summary>
        /// Raises a task status event
        /// </summary>
        /// <param name="status"></param>
        private void RaiseTasksCompletedEvent(string status)
        {
            TaskStatusEventArg statusArgs = new TaskStatusEventArg
            {
                Status = status
            };

            TasksCompleted?.Invoke(statusArgs);
        }

        /// <summary>
        /// Loads a task collection from a file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private ICollection<DevOpsTask> LoadTasksFromFile(string filePath)
        {
            var tasks = GetTasksFromFile(filePath);
            if (tasks == null)
            {
                throw new DevOpsTaskException("Unable to load tasks from path provided");
            }

            return tasks;
        }

        /// <summary>
        /// Binds the methods declared in the task collection
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        private int BindFuncs(IEnumerable<DevOpsTask> tasks)
        {
            var funcs = 0;
            var rootPath = ConfigurationManager.AppSettings["buildTasksPath"];
            if (string.IsNullOrEmpty(rootPath))
            {
                throw new DevOpsTaskException("Unable to load Build tasks root path [buildTasksPath]");
            }

            foreach (var item in tasks)
            {
                var path = Path.Combine(rootPath, item.Assembly);
                Assembly asm = Assembly.LoadFrom(path);
                Type t = asm.GetType(item.Namespace);
                var methodInfo = t.GetMethod(item.Method);
                var instance = Activator.CreateInstance(t);
                if (instance == null)
                {
                    throw new DevOpsTaskException("Unable to create instance of type");
                }

                if(instance is NotifyTask)
                {
                    var notifyTask = instance as NotifyTask;
                    notifyTask.NotifyTaskOutputData += (o, e) =>
                      {
                          BroadcastStatus(e.Message);
                      };
                }

                Func<Task<DevOpsTaskStatus>> func = (Func<Task<DevOpsTaskStatus>>)Delegate.CreateDelegate(typeof(Func<Task<DevOpsTaskStatus>>), instance, methodInfo);
                item.func = func;
                funcs++;
            }

            return funcs;
        }        

        /// <summary>
        /// Raises an exception given a DevOpsTask and Exception instance
        /// </summary>
        /// <param name="task"></param>
        /// <param name="ex"></param>
        private void RaiseException(DevOpsTask task, Exception ex = null)
        {
            var devOpsEx = new DevOpsTaskException(task, ex.Message);

            throw devOpsEx;
        }

        /// <summary>
        /// Raises an exception given an Exception instance
        /// </summary>
        /// <param name="ex"></param>
        private void RaiseException(Exception ex)
        {
            var devOpsEx = new DevOpsTaskException(ex.Message);

            throw devOpsEx;
        }

        private ICollection<DevOpsTask> GetTasksFromFile(string filePath)
        {
            try
            {
                var tasks = XmlSerialisationManager<DevOpsTask>.DeserializeObjects(filePath);
                if (tasks != null)
                {
                    return tasks;
                }
            }
            catch (Exception e)
            {
                RaiseException(e);
            }

            return null;
        }

        /// <summary>
        /// Serializes the tasks to a file
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="filePath"></param>
        private void SerializeTasksToFile(IEnumerable<DevOpsTask> tasks, string filePath)
        {
            try
            {
                XmlSerialisationManager<DevOpsTask>.SerializeObjects(tasks.ToList(), filePath);
            }
            catch (Exception e)
            {
                RaiseException(e);
            }
        }

    }
}
