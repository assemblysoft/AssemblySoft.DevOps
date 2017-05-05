using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AssemblySoft.Serialization;


namespace AssemblySoft.DevOps
{

    public partial class TaskRunner
    {
        public TaskRunner()
        {
        }

        public delegate void TaskStatusEventHandler(TaskStatusEventArg e);
        public event TaskStatusEventHandler TaskStatus;

        Assembly _invokingAssembly;

        IEnumerable<DevOpsTask> _tasks;
        public IEnumerable<DevOpsTask> GetDevOpsTaskWithState()
        {
            return _tasks;
        }

        private void BroadcastStatus(string msg)
        {
            RaiseTaskStatusEvent(msg);
        }

        private void RaiseTaskStatusEvent(string status)
        {
            var statusArgs = new TaskStatusEventArg
            {
                Status = status
            };

            TaskStatus?.Invoke(statusArgs);
        }       

        private ICollection<DevOpsTask> LoadTasksFromFile(string filePath)
        {
            var tasks = GetTasksFromFile(filePath);
            if (tasks == null)
            {
                throw new DevOpsTaskException("Unable to load tasks from path provided");
            }

            return tasks;
        }        

        private int BindActions(IEnumerable<DevOpsTask> tasks)
        {
            var actions = 0;
            foreach (var item in tasks)
            {
                var path = Path.Combine(ConfigurationManager.AppSettings["buildTasksPath"], item.AssemblyPath);
                Assembly asm = Assembly.LoadFrom(path);
                Type t = asm.GetType(item.Namespace);
                var methodInfo = t.GetMethod(item.Method);
                var instance = Activator.CreateInstance(t);
                if(instance == null)
                {
                    throw new DevOpsTaskException("Unable to create instance of type");
                }

                Func<DevOpsTaskStatus, Task> action = (Func<DevOpsTaskStatus, Task>)Delegate.CreateDelegate(typeof(Func<DevOpsTaskStatus, Task>), instance, methodInfo);
                item.runTask = action;

                actions++;
            }

            return actions;
        }
       
        public async Task Run(string filePath)
        {
            var tasks = LoadTasksFromFile(filePath);

            var actionCount = BindActions(tasks.Where(t => t.Enabled == true)); //filter out tasks marked as disabled
            if (actionCount <= 0)
            {
                throw new DevOpsTaskException("Unable to link methods to tasks");
            }

            await Run(tasks);

        }        

        public async Task Run(IEnumerable<DevOpsTask> tasks)
        {
            _tasks = tasks.ToList();
            DevOpsTask currentTask;

            //split into groups of orders
            List<List<DevOpsTask>> orderGrouped = new List<List<DevOpsTask>>();
            var taskGroups = _tasks.GroupBy(t => t.Order);
            
            foreach(var taskSet in taskGroups)
            {              

                List<Task> tasksCollection = new List<Task>();

                foreach (var task in taskSet)
                {
                    currentTask = task;

                    try
                    {
                        if (task.Enabled)
                        {
                            BroadcastStatus(String.Format("Starting {0}", task.Description));
                            task.Status = DevOpsTaskStatus.Started;
                        }
                        else
                        {
                            BroadcastStatus(String.Format("Skipped {0}", task.Description));
                            task.Status = DevOpsTaskStatus.Skipped;
                        }

                        if (task.Enabled)
                        {                            

                            await Task.Factory.StartNew(() =>
                            {
                                var start = System.Environment.TickCount;
                                task.runTask?.Invoke(task.Status);
                                var stop = System.Environment.TickCount;

                                double elapsedTimeSeconds = (stop - start) / 1000;
                                task.LastExecutionDuration = string.Format("{0:#,##0.00}", elapsedTimeSeconds);

                            }).ContinueWith((antecedant) =>
                            {

                            });

                            //await Task.Run(task.runAction);
                            tasksCollection.Add(Task.Run( () =>
                            {
                                var start = System.Environment.TickCount;
                                task.runTask?.Invoke(task.Status);
                                var stop = System.Environment.TickCount;

                                double elapsedTimeSeconds = (stop - start) / 1000;
                                task.LastExecutionDuration = string.Format("{0:#,##0.00}",elapsedTimeSeconds);
                            }
                            ));                           
                           
                        }
                    }
                    catch (Exception ex)
                    {
                        //ToDo: change method of logging as this may also cause an exception
                        throw new DevOpsTaskException(currentTask, ex.Message);
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
                catch {

                }

                if (t.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    foreach (var task in taskSet)
                    {
                        if (task.Enabled && task.Status == DevOps.DevOpsTaskStatus.Completed)
                        {
                            BroadcastStatus(String.Format("Completed {0}", task.Description));
                            task.Enabled = false;
                        }
                    }
                }
                else if (t.Status == System.Threading.Tasks.TaskStatus.Faulted)
                {

                }

            }          

        }

        public void RaiseException(DevOpsTask task, Exception ex = null)
        {
            var devOpsEx = new DevOpsTaskException(task, ex.Message);

            throw devOpsEx;
        }

        public void RaiseException(Exception ex)
        {
            var devOpsEx = new DevOpsTaskException(ex.Message);

            throw devOpsEx;
        }

        public ICollection<DevOpsTask> GetTasksFromFile(string filePath)
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

        public void SerializeTasksToFile(IEnumerable<DevOpsTask> tasks, string filePath)
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
