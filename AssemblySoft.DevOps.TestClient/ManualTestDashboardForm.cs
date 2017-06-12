using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssemblySoft.DevOps.TestClient
{
    public partial class ManualTestDashboardForm : Form
    {
        TaskRunner _taskRunner;
        int _counter;

        public ManualTestDashboardForm()
        {
            InitializeComponent();

            _taskRunner = new TaskRunner();
            _taskRunner.TaskStatus += _taskRunner_TaskStatus;
        }

        private void _taskRunner_TaskStatus(TaskRunner.TaskStatusEventArg e)
        {
            AddStatus(e.Status);
        }

        private void button_Start_Tasks_Click(object sender, EventArgs e)
        {
            try
            {                
                //button_Start_Tasks.Enabled = false;
                Cursor = Cursors.WaitCursor;
                AddStatus("Start");
                _counter++;
                labelConcurrentInstances.Text = _counter.ToString();
                
                Task t1 = new Task(() =>
                {
                    _taskRunner.Run(ConfigurationManager.AppSettings["tasksPath"]);                    
                });
                
                Task t2 = t1.ContinueWith((ante) =>
                {
                    AddStatus("Complete");

                    _counter--;
                    labelConcurrentInstances.Text = _counter.ToString();
                    if (_counter == 0)
                    {
                        Cursor = Cursors.Arrow;
                    }

                    //button_Start_Tasks.Enabled = true;
                    
                },TaskScheduler.FromCurrentSynchronizationContext());           
                
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
            }            
        }

        private void HandleException(Exception e)
        {
            if (e is DevOpsTaskException)
            {
                var devOpsEx = e as DevOpsTaskException;
                AddStatus(string.Format("{0} failed with error {1}", devOpsEx.Task != null ? devOpsEx.Task.Description : string.Empty, devOpsEx.Message));
            }
            else
            {
                AddStatus(string.Format("Failed with error {0}", e.Message));
            }
        }    
        

    }
}
