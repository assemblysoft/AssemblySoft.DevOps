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
            this.SuspendLayout();
            // 
            // button_Start_Tasks
            // 
            this.button_Start_Tasks.Location = new System.Drawing.Point(12, 12);
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
            // ManualTestDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 351);
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
    }
}