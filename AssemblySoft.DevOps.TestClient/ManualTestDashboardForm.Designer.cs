namespace AssemblySoft.DevOps.TestClient
{
    partial class ManualTestDashboardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_Start_Tasks = new System.Windows.Forms.Button();
            this.listBox_status = new System.Windows.Forms.ListBox();
            this.labelTaskStatus = new System.Windows.Forms.Label();
            this.labelConcurrentInstances = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelStatusResult = new System.Windows.Forms.Label();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.button_ClearOutput = new System.Windows.Forms.Button();
            this.buttonCancelTasks = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Start_Tasks
            // 
            this.button_Start_Tasks.Location = new System.Drawing.Point(7, 8);
            this.button_Start_Tasks.Name = "button_Start_Tasks";
            this.button_Start_Tasks.Size = new System.Drawing.Size(128, 45);
            this.button_Start_Tasks.TabIndex = 0;
            this.button_Start_Tasks.Text = "Start Running Tasks";
            this.button_Start_Tasks.UseVisualStyleBackColor = true;
            this.button_Start_Tasks.Click += new System.EventHandler(this.button_Start_Tasks_Click);
            // 
            // listBox_status
            // 
            this.listBox_status.FormattingEnabled = true;
            this.listBox_status.Location = new System.Drawing.Point(12, 75);
            this.listBox_status.Name = "listBox_status";
            this.listBox_status.Size = new System.Drawing.Size(948, 160);
            this.listBox_status.TabIndex = 1;
            // 
            // labelTaskStatus
            // 
            this.labelTaskStatus.AutoSize = true;
            this.labelTaskStatus.Location = new System.Drawing.Point(300, 43);
            this.labelTaskStatus.Name = "labelTaskStatus";
            this.labelTaskStatus.Size = new System.Drawing.Size(96, 13);
            this.labelTaskStatus.TabIndex = 4;
            this.labelTaskStatus.Text = "Running Instances";
            // 
            // labelConcurrentInstances
            // 
            this.labelConcurrentInstances.AutoSize = true;
            this.labelConcurrentInstances.Location = new System.Drawing.Point(402, 44);
            this.labelConcurrentInstances.Name = "labelConcurrentInstances";
            this.labelConcurrentInstances.Size = new System.Drawing.Size(0, 13);
            this.labelConcurrentInstances.TabIndex = 5;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(526, 44);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(37, 13);
            this.labelStatus.TabIndex = 6;
            this.labelStatus.Text = "Status";
            // 
            // labelStatusResult
            // 
            this.labelStatusResult.AutoSize = true;
            this.labelStatusResult.Location = new System.Drawing.Point(610, 44);
            this.labelStatusResult.Name = "labelStatusResult";
            this.labelStatusResult.Size = new System.Drawing.Size(24, 13);
            this.labelStatusResult.TabIndex = 7;
            this.labelStatusResult.Text = "Idle";
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Location = new System.Drawing.Point(12, 257);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStatus.Size = new System.Drawing.Size(948, 152);
            this.textBoxStatus.TabIndex = 8;
            this.textBoxStatus.VisibleChanged += new System.EventHandler(this.textBoxStatus_VisibleChanged);
            // 
            // button_ClearOutput
            // 
            this.button_ClearOutput.Location = new System.Drawing.Point(832, 12);
            this.button_ClearOutput.Name = "button_ClearOutput";
            this.button_ClearOutput.Size = new System.Drawing.Size(128, 45);
            this.button_ClearOutput.TabIndex = 9;
            this.button_ClearOutput.Text = "Clear Output";
            this.button_ClearOutput.UseVisualStyleBackColor = true;
            this.button_ClearOutput.Click += new System.EventHandler(this.button_ClearOutput_Click);
            // 
            // buttonCancelTasks
            // 
            this.buttonCancelTasks.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.buttonCancelTasks.Enabled = false;
            this.buttonCancelTasks.Location = new System.Drawing.Point(676, 12);
            this.buttonCancelTasks.Name = "buttonCancelTasks";
            this.buttonCancelTasks.Size = new System.Drawing.Size(128, 45);
            this.buttonCancelTasks.TabIndex = 10;
            this.buttonCancelTasks.Text = "Cancel Running Tasks";
            this.buttonCancelTasks.UseVisualStyleBackColor = true;
            this.buttonCancelTasks.Click += new System.EventHandler(this.buttonCancelTasks_Click);
            // 
            // ManualTestDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 434);
            this.Controls.Add(this.buttonCancelTasks);
            this.Controls.Add(this.button_ClearOutput);
            this.Controls.Add(this.textBoxStatus);
            this.Controls.Add(this.labelStatusResult);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelConcurrentInstances);
            this.Controls.Add(this.labelTaskStatus);
            this.Controls.Add(this.listBox_status);
            this.Controls.Add(this.button_Start_Tasks);
            this.Name = "ManualTestDashboardForm";
            this.Text = "Manual Test Dashboard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Start_Tasks;
        private System.Windows.Forms.ListBox listBox_status;
        private System.Windows.Forms.Label labelTaskStatus;
        private System.Windows.Forms.Label labelConcurrentInstances;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelStatusResult;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Button button_ClearOutput;
        private System.Windows.Forms.Button buttonCancelTasks;
    }
}
