namespace SEYRDesktop
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.NumPxPerMicron = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.NumFrame = new System.Windows.Forms.NumericUpDown();
            this.BtnRestartAndRun = new System.Windows.Forms.Button();
            this.BtnRunAll = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.BtnOpenComposer = new System.Windows.Forms.Button();
            this.BtnForcePattern = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.BtnOpenDir = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPxPerMicron)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.NumPxPerMicron, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.NumFrame, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.BtnRestartAndRun, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.BtnRunAll, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.BtnStop, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.BtnOpenComposer, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.BtnForcePattern, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.ProgressBar, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.BtnOpenDir, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(435, 122);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // NumPxPerMicron
            // 
            this.NumPxPerMicron.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumPxPerMicron.AutoSize = true;
            this.NumPxPerMicron.DecimalPlaces = 3;
            this.NumPxPerMicron.Location = new System.Drawing.Point(79, 85);
            this.NumPxPerMicron.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumPxPerMicron.Name = "NumPxPerMicron";
            this.NumPxPerMicron.Size = new System.Drawing.Size(167, 20);
            this.NumPxPerMicron.TabIndex = 16;
            this.NumPxPerMicron.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumPxPerMicron.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(2, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 34);
            this.label1.TabIndex = 15;
            this.label1.Text = "Pixels/Micron";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(2, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 34);
            this.label4.TabIndex = 11;
            this.label4.Text = "Frame #";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumFrame
            // 
            this.NumFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumFrame.AutoSize = true;
            this.NumFrame.Enabled = false;
            this.NumFrame.Location = new System.Drawing.Point(79, 51);
            this.NumFrame.Name = "NumFrame";
            this.NumFrame.Size = new System.Drawing.Size(167, 20);
            this.NumFrame.TabIndex = 3;
            this.NumFrame.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumFrame.ValueChanged += new System.EventHandler(this.NumFrame_ValueChanged);
            // 
            // BtnRestartAndRun
            // 
            this.BtnRestartAndRun.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRestartAndRun.AutoSize = true;
            this.BtnRestartAndRun.Enabled = false;
            this.BtnRestartAndRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRestartAndRun.Location = new System.Drawing.Point(357, 13);
            this.BtnRestartAndRun.Name = "BtnRestartAndRun";
            this.BtnRestartAndRun.Size = new System.Drawing.Size(76, 28);
            this.BtnRestartAndRun.TabIndex = 21;
            this.BtnRestartAndRun.Text = "Restart Run";
            this.BtnRestartAndRun.UseVisualStyleBackColor = true;
            this.BtnRestartAndRun.Click += new System.EventHandler(this.BtnRestartAndRun_Click);
            // 
            // BtnRunAll
            // 
            this.BtnRunAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRunAll.AutoSize = true;
            this.BtnRunAll.Enabled = false;
            this.BtnRunAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRunAll.Location = new System.Drawing.Point(357, 47);
            this.BtnRunAll.Name = "BtnRunAll";
            this.BtnRunAll.Size = new System.Drawing.Size(76, 28);
            this.BtnRunAll.TabIndex = 4;
            this.BtnRunAll.Text = "Run";
            this.BtnRunAll.UseVisualStyleBackColor = true;
            this.BtnRunAll.Click += new System.EventHandler(this.BtnRunAll_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnStop.AutoSize = true;
            this.BtnStop.Enabled = false;
            this.BtnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStop.Location = new System.Drawing.Point(357, 81);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(76, 28);
            this.BtnStop.TabIndex = 13;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // BtnOpenComposer
            // 
            this.BtnOpenComposer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOpenComposer.AutoSize = true;
            this.BtnOpenComposer.Enabled = false;
            this.BtnOpenComposer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOpenComposer.Location = new System.Drawing.Point(252, 81);
            this.BtnOpenComposer.Name = "BtnOpenComposer";
            this.BtnOpenComposer.Size = new System.Drawing.Size(99, 28);
            this.BtnOpenComposer.TabIndex = 1;
            this.BtnOpenComposer.Text = "Open Composer";
            this.BtnOpenComposer.UseVisualStyleBackColor = true;
            this.BtnOpenComposer.Click += new System.EventHandler(this.BtnOpenComposer_Click);
            // 
            // BtnForcePattern
            // 
            this.BtnForcePattern.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnForcePattern.AutoSize = true;
            this.BtnForcePattern.Enabled = false;
            this.BtnForcePattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnForcePattern.Location = new System.Drawing.Point(252, 47);
            this.BtnForcePattern.Name = "BtnForcePattern";
            this.BtnForcePattern.Size = new System.Drawing.Size(99, 28);
            this.BtnForcePattern.TabIndex = 20;
            this.BtnForcePattern.Text = "Force Pattern";
            this.BtnForcePattern.UseVisualStyleBackColor = true;
            this.BtnForcePattern.Click += new System.EventHandler(this.BtnForcePattern_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.ProgressBar, 2);
            this.ProgressBar.Location = new System.Drawing.Point(2, 15);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(244, 23);
            this.ProgressBar.TabIndex = 23;
            // 
            // BtnOpenDir
            // 
            this.BtnOpenDir.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOpenDir.AutoSize = true;
            this.BtnOpenDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOpenDir.Location = new System.Drawing.Point(252, 13);
            this.BtnOpenDir.Name = "BtnOpenDir";
            this.BtnOpenDir.Size = new System.Drawing.Size(99, 28);
            this.BtnOpenDir.TabIndex = 14;
            this.BtnOpenDir.Text = "Open Dir";
            this.BtnOpenDir.UseVisualStyleBackColor = true;
            this.BtnOpenDir.Click += new System.EventHandler(this.BtnOpenDir_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 122);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(416, 148);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "v1.3.30";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPxPerMicron)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumFrame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button BtnOpenComposer;
        private System.Windows.Forms.NumericUpDown NumFrame;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button BtnRunAll;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Button BtnOpenDir;
        private System.Windows.Forms.NumericUpDown NumPxPerMicron;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnForcePattern;
        private System.Windows.Forms.Button BtnRestartAndRun;
        private System.Windows.Forms.ProgressBar ProgressBar;
    }
}

