using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssemblySoft.DevOps.TestClient
{
    public partial class ManualTestDashboardForm : Form
    {
        TaskRunner _taskRunner;
        int _counter;
        CancellationTokenSource _cts = new CancellationTokenSource();

        public ManualTestDashboardForm()
        {
            InitializeComponent();

            _taskRunner = new TaskRunner();
            _taskRunner.TaskStatus += (e) => AddStatus(e.Status);
            _taskRunner.TasksCompleted += (e) =>
             {
                 if (!string.IsNullOrEmpty(e.Status))
                 {
                     if (e.Status != DevOpsTaskStatus.Completed.ToString())
                     {
                         AddStatus("Task run was aborted!");
                         return;
                     }
                 }
                 else
                 {
                     AddStatus(e.Status);
                 }
             };
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
                //button_Start_Tasks.Enabled = false; //uncomment to prevent multiple instances
                Cursor = Cursors.WaitCursor;
                AddStatus("Start");
                labelStatusResult.Text = "Running";
                _counter++;
                buttonCancelTasks.Enabled = true;
                labelConcurrentInstances.Text = _counter.ToString();
                
                var token = _cts.Token;

                Task<DevOpsTaskStatus> t1 = new Task<DevOpsTaskStatus>(() =>
                {
                    return _taskRunner.Run(token,ConfigurationManager.AppSettings["tasksPath"]);                    
                },token);
                
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

                    //button_Start_Tasks.Enabled = true; //uncomment to prevent multiple instances                                       

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
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                var tasks = _taskRunner.GetDevOpsTaskWithState();
                // _taskRunner.SerializeTasksToFile(tasks, ConfigurationManager.AppSettings["tasksPath"]);
            }
        }

        delegate void AddStatusCallback(string text);
        private void AddStatus(string result)
        {
            if(string.IsNullOrEmpty(result))
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
