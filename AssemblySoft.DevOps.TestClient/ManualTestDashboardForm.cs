using AssemblySoft.IO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssemblySoft.DevOps.TestClient
{
    public partial class ManualTestDashboardForm : Form
    {     
        
        
        int _counter;
        CancellationTokenSource _cts = new CancellationTokenSource();

        public ManualTestDashboardForm()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// Start Tasks click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Start_Tasks_Click(object sender, EventArgs e)
        {            
            try
            {
                button_Start_Tasks.Enabled = false; //uncomment to prevent multiple instances
                Cursor = Cursors.WaitCursor;
                AddStatus("Start");
                labelStatusResult.Text = "Running";
                _counter++;
                buttonCancelTasks.Enabled = true;
                labelConcurrentInstances.Text = _counter.ToString();

                var token = _cts.Token;
                string runPath = InitialiseBuildRun();

                var taskRunner = new TaskRunner(runPath);
                taskRunner.TaskStatus += (t) => AddStatus(t.Status);
                taskRunner.TasksCompleted += (t) =>
                {
                    if (!string.IsNullOrEmpty(t.Status))
                    {
                        if (t.Status != DevOpsTaskStatus.Completed.ToString())
                        {
                            AddStatus("Task run was aborted!");
                            return;
                        }
                    }
                    else
                    {
                        AddStatus(t.Status);
                    }
                };

                Task<DevOpsTaskStatus> t1 = new Task<DevOpsTaskStatus>(() =>
                {
                    return taskRunner.Run(token, Path.Combine(runPath, "install.tasks"));

                }, token);

                Task t2 = t1.ContinueWith((ante) =>
                {

                    AddStatus("Stopped");

                    _counter--;
                    labelConcurrentInstances.Text = _counter.ToString();
                    if (_counter == 0)
                    {
                        Cursor = Cursors.Arrow;
                        labelStatusResult.Text = "Idle";
                        buttonCancelTasks.Enabled = false;
                    }

                    button_Start_Tasks.Enabled = true; //uncomment to prevent multiple instances                                       

                    try
                    {
                        AddStatus(string.Format("Task runner returned {0}", ante.Result.ToString())); //harvest result, rethrow if necessary
                    }
                    catch (AggregateException ae)
                    {
                        HandleException(ae);
                    }


                }, TaskScheduler.FromCurrentSynchronizationContext());

                t1.Start();


            }
            catch (DevOpsTaskException ex)
            {
                HandleException(ex);
            }
            catch (AggregateException ae)
            {
                HandleException(ae);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //var tasks = taskRunner.GetDevOpsTaskWithState();
                // _taskRunner.SerializeTasksToFile(tasks, ConfigurationManager.AppSettings["tasksPath"]);
            }
        }

        private string InitialiseBuildRun()
        {
            var tasksDestinationPath = ConfigurationManager.AppSettings["tasksRunnerRootPath"];

            //root path for the source task artifacts
            var tasksSourcePath = ConfigurationManager.AppSettings["tasksSourcePath"];

            //create new directory for tasks to run
            if (!Directory.Exists(tasksDestinationPath))
            {
                Directory.CreateDirectory(tasksDestinationPath);
            }

            int latestCount = GetNextBuildNumber(tasksDestinationPath);

            var runPath = Path.Combine(tasksDestinationPath, string.Format("{0}", latestCount));
            Directory.CreateDirectory(runPath);

            //generate basic log to identify task run
            string path = Path.Combine(runPath, string.Format("{0}", "build.log"));
            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(string.Format("{0} Ver: {1}", "Build Runner version", "2.1"));
                    sw.WriteLine(string.Format("{0} {1}", DateTime.UtcNow, runPath));
                }
            }

            //copy build artifacts                
            DirectoryClient.DirectoryCopy(tasksSourcePath, runPath, true);
            return runPath;
        }

        private static int GetNextBuildNumber(string rootPath)
        {
            DirectoryInfo info = new DirectoryInfo(rootPath);
            var directories = info.GetDirectories();
            int latestCount = 0;
            foreach (var dir in directories)
            {
                if (int.TryParse(dir.Name, out int res))
                {
                    if (res > latestCount)
                    {
                        latestCount = res;
                    }
                }

            }
            latestCount++;
            return latestCount;
        }

        delegate void AddStatusCallback(string text);
        private void AddStatus(string result)
        {
            if (string.IsNullOrEmpty(result))
            {
                return;
            }

            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                AddStatusCallback callback = new AddStatusCallback(AddStatus);
                this.Invoke(callback, new object[] { result });
            }
            else
            {
                listBox_status.Items.Add(result);
                textBoxStatus.AppendLine(result);
            }
        }

        /// <summary>
        /// Central location for handling exceptions
        /// </summary>
        /// <param name="e"></param>
        private void HandleException(Exception e)
        {

            if (e is AggregateException)
            {
                StringBuilder exBuilder = new StringBuilder();
                var aggEx = e as AggregateException;

                if (aggEx.InnerException is OperationCanceledException)
                {
                    AddStatus("Task Run cancelled");
                    labelStatusResult.Text = "Cancelled";
                }
                else
                {
                    aggEx = aggEx.Flatten();
                    foreach (Exception ex in aggEx.InnerExceptions)
                    {
                        exBuilder.AppendLine(ex.Message); //.AppendLine(ex.StackTrace);
                    }

                    AddStatus(string.Format("Failed with Ae Error: {0}", exBuilder.ToString()));
                    labelStatusResult.Text = "Aborted";
                }
            }
            else if (e is DevOpsTaskException)
            {
                var devOpsEx = e as DevOpsTaskException;
                AddStatus(string.Format("{0} failed with error {1}", devOpsEx.Task != null ? devOpsEx.Task.Description : string.Empty, devOpsEx.Message));
                labelStatusResult.Text = "Aborted";
            }
            else
            {
                AddStatus(string.Format("Failed with error {0}", e.Message));
                labelStatusResult.Text = "Aborted";
            }

        }

        private void textBoxStatus_VisibleChanged(object sender, EventArgs e)
        {
            if (textBoxStatus.Visible)
            {
                textBoxStatus.SelectionStart = textBoxStatus.TextLength;
                textBoxStatus.ScrollToCaret();
            }
        }

        private void button_ClearOutput_Click(object sender, EventArgs e)
        {
            textBoxStatus.Clear();
            listBox_status.Items.Clear();
        }

        private void buttonCancelTasks_Click(object sender, EventArgs e)
        {
            _cts.Cancel(); //cancel all instances

            _cts = new CancellationTokenSource(); //create new instance for future tasks
        }
    }
}
