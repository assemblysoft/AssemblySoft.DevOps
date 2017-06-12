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

        //tasksCollection.Add(
        //Task.Factory.StartNew(() =>
        //{
        //    var start = System.Environment.TickCount;

        //    if (devOpsTask.func != null)
        //    {
        //        devOpsTask.Status = devOpsTask.func.Invoke().Result;
        //    }

        //    var stop = System.Environment.TickCount;
        //    double elapsedTimeSeconds = (stop - start) / 1000;
        //    devOpsTask.LastExecutionDuration = string.Format("{0:#,##0.00}", elapsedTimeSeconds);

        //    if (devOpsTask.Enabled) //only report on enabled tasks                    
        //    {
        //        //BroadcastStatus(String.Format("{0} {1} ({2}) {3} {4}", devOpsTask.Status, devOpsTask.Description, devOpsTask.Order, devOpsTask.LastExecutionDuration, DateTime.UtcNow));
        //        //task.Enabled = false; //updates to disabled
        //    }

        //}
        //));

        //void PerformTest_TaskWaitAll(List<DevOpsTask> workers)
        //{
        //    //Task.WaitAll(workers.Select<Func<>>(worker => worker.Run()).ToArray());
        //}



        //async RunTask

        /*
        foreach (var task in _tasks.OrderBy(x => x.Order))
        {
            currentTask = task;

            try
            {
                if (task.Enabled)
                    BroadcastStatus(String.Format("Starting {0}", task.Description));
                else
                    BroadcastStatus(String.Format("Skipped {0}", task.Description));

                if (task.Enabled)
                {
                    task.Status = DevOpsTaskStatus.Started;
                    task.Run(ref task.Status);                    


                    if (task.Enabled && task.Status == DevOps.DevOpsTaskStatus.Completed)
                        BroadcastStatus(String.Format("Completed {0}", task.Description));

                    task.Enabled = false;                        
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
        */

        public async Task RunAsync(IEnumerable<DevOpsTask> devOpsTasks)
        {
            _devOpsTasks = devOpsTasks.ToList();
            DevOpsTask currentDevOpsTask;

            //split into groups of orders
            List<List<DevOpsTask>> orderGrouped = new List<List<DevOpsTask>>();
            var devOpsTaskGroups = _devOpsTasks.GroupBy(t => t.Order);

            foreach (var devOpsTaskSet in devOpsTaskGroups)
            {

                //List<Task<Func<DevOpsTaskStatus, Task>>> tks = new List<Task<Func<DevOpsTaskStatus, Task>>>();

                List<Task> tasksCollection = new List<Task>();

                foreach (var devOpsTask in devOpsTaskSet)
                {
                    currentDevOpsTask = devOpsTask;

                    try
                    {
                        if (devOpsTask.Enabled)
                        {
                            BroadcastStatus(String.Format("Starting {0}", devOpsTask.Description));
                            devOpsTask.Status = DevOpsTaskStatus.Started;
                        }
                        else
                        {
                            BroadcastStatus(String.Format("Skipped {0}", devOpsTask.Description));
                            devOpsTask.Status = DevOpsTaskStatus.Skipped;
                        }

                        List<Task> tasks = new List<Task>();
                        if (devOpsTask.Enabled)
                        {
                            //tks.Add(Task.Adnew Task(task.runTask));
                            //task.Run(ref task.Status);
                            //await task.Run(task.Status);

                            tasksCollection.Add(
                            await Task.Factory.StartNew(async () =>
                            {
                                var start = System.Environment.TickCount;
                                //devOpsTask.runTask?.Invoke(devOpsTask.Status);

                                // devOpsTask.func?.Invoke();

                                if (devOpsTask.func != null)
                                {
                                    var result = await devOpsTask.func.Invoke();


                                }

                                var stop = System.Environment.TickCount;

                                double elapsedTimeSeconds = (stop - start) / 1000;
                                devOpsTask.LastExecutionDuration = string.Format("{0:#,##0.00}", elapsedTimeSeconds);

                            }));

                            //.ContinueWith((antecedant) =>
                            //{

                            //}));


                            //Task.WaitAll(tasksCollection.ToArray());

                            //await Task.Run(task.runAction);
                            //tasksCollection.Add(Task.Run( () =>
                            //{
                            //    var start = System.Environment.TickCount;
                            //    devOpsTask.runTask?.Invoke(devOpsTask.Status);
                            //    var stop = System.Environment.TickCount;

                            //    double elapsedTimeSeconds = (stop - start) / 1000;
                            //    devOpsTask.LastExecutionDuration = string.Format("{0:#,##0.00}",elapsedTimeSeconds);
                            //}
                            //));

                            //if (task.Enabled && task.Status == DevOps.DevOpsTaskStatus.Completed)
                            //{
                            //    BroadcastStatus(String.Format("Completed {0}", task.Description));
                            //    task.Enabled = false;
                            //}
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

                Task.WaitAll(tasksCollection.ToArray());

                var x = "continue";
                //Task t = Task.WhenAll(tasksCollection);
                //try
                //{
                //    t.Wait();
                //}
                //catch {

                //}

                //if (t.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                //{
                //    foreach (var task in devOpsTaskSet)
                //    {
                //        if (task.Enabled && task.Status == DevOps.DevOpsTaskStatus.Completed)
                //        {
                //            BroadcastStatus(String.Format("Completed {0}", task.Description));
                //            task.Enabled = false;
                //        }
                //    }
                //}
                //else if (t.Status == System.Threading.Tasks.TaskStatus.Faulted)
                //{

                //}

            }

            //void PerformTest_TaskWaitAll(List<DevOpsTask> workers)
            //{
            //    //Task.WaitAll(workers.Select<Func<>>(worker => worker.Run()).ToArray());
            //}



            //async RunTask

            /*
            foreach (var task in _tasks.OrderBy(x => x.Order))
            {
                currentTask = task;

                try
                {
                    if (task.Enabled)
                        BroadcastStatus(String.Format("Starting {0}", task.Description));
                    else
                        BroadcastStatus(String.Format("Skipped {0}", task.Description));

                    if (task.Enabled)
                    {
                        task.Status = DevOpsTaskStatus.Started;
                        task.Run(ref task.Status);                    


                        if (task.Enabled && task.Status == DevOps.DevOpsTaskStatus.Completed)
                            BroadcastStatus(String.Format("Completed {0}", task.Description));

                        task.Enabled = false;                        
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
            */

        }

        private int BindActions(IEnumerable<DevOpsTask> tasks)
        {
            var actions = 0;
            foreach (var item in tasks)
            {
                var path = Path.Combine(ConfigurationManager.AppSettings["buildTasksPath"], item.Assembly);
                Assembly asm = Assembly.LoadFrom(path);
                Type t = asm.GetType(item.Namespace);
                var methodInfo = t.GetMethod(item.Method);
                var instance = Activator.CreateInstance(t);
                if (instance == null)
                {
                    throw new DevOpsTaskException("Unable to create instance of type");
                }

                Func<DevOpsTaskStatus, Task> action = (Func<DevOpsTaskStatus, Task>)Delegate.CreateDelegate(typeof(Func<DevOpsTaskStatus, Task>), instance, methodInfo);
                item.runTask = action;

                actions++;
            }

            return actions;
        }

        private int HookupValActions(IEnumerable<DevOpsTask> tasks)
        {
            var actions = 0;
            foreach (var item in tasks)
            {
                var path = Path.Combine(ConfigurationManager.AppSettings["buildTasksPath"], item.Assembly);
                Assembly asm = Assembly.LoadFrom(path);
                Type t = asm.GetType(item.Namespace);
                var methodInfo = t.GetMethod(item.Method);
                var o = Activator.CreateInstance(t);
                Action<DevOpsTaskStatus> action = (Action<DevOpsTaskStatus>)Delegate.CreateDelegate(typeof(Action<DevOpsTaskStatus>), o, methodInfo);
                item.runActionVal = action;

                actions++;
            }

            return actions;
        }

        private int HookupActions(IEnumerable<DevOpsTask> tasks)
        {
            var actions = 0;
            foreach (var item in tasks)
            {
                var path = Path.Combine(ConfigurationManager.AppSettings["buildTasksPath"], item.Assembly);
                Assembly asm = Assembly.LoadFrom(path);
                Type t = asm.GetType(item.Namespace);
                var methodInfo = t.GetMethod(item.Method);
                var o = Activator.CreateInstance(t);
                Action action = (Action)Delegate.CreateDelegate(typeof(Action), o, methodInfo);
                item.runAction = action;

                actions++;
            }

            return actions;
        }


        //public void Run(IEnumerable<DevOpsTask> tasks, Assembly invokingAssembly)
        //{
        //    _invokingAssembly = invokingAssembly;

        //    Run(tasks.ToList());
        //}

        //private int HookupRefActions(IEnumerable<DevOpsTask> tasks)
        //{
        //    var actions = 0;
        //    foreach (var item in tasks)
        //    {
        //        var path = Path.Combine(ConfigurationManager.AppSettings["buildTasksPath"], item.AssemblyPath);
        //        Assembly asm = Assembly.LoadFrom(path);
        //        Type t = asm.GetType(item.Namespace);
        //        var methodInfo = t.GetMethod(item.Method);
        //        var o = Activator.CreateInstance(t);
        //        DevOpsTask.ActionRef<DevOpsTaskStatus> action = (DevOpsTask.ActionRef<DevOpsTaskStatus>)Delegate.CreateDelegate(typeof(DevOpsTask.ActionRef<DevOpsTaskStatus>), o, methodInfo);
        //        item.runActionRef = action;

        //        actions++;
        //    }

        //    return actions;
        //}


        #endregion
    }
}
