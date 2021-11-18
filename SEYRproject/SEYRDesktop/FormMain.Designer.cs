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
            this.btnOpenDir = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnOpenComposer = new System.Windows.Forms.Button();
            this.btnClearData = new System.Windows.Forms.Button();
            this.btnOpenGIF = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numPatternFollowDelay = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numFrame = new System.Windows.Forms.NumericUpDown();
            this.btnRunAll = new System.Windows.Forms.Button();
            this.numPatternFollowInterval = new System.Windows.Forms.NumericUpDown();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPatternFollowDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPatternFollowInterval)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.btnOpenDir, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnStop, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenComposer, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnClearData, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenGIF, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.numPatternFollowDelay, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.numFrame, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnRunAll, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.numPatternFollowInterval, 4, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(325, 149);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnOpenDir
            // 
            this.btnOpenDir.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenDir.AutoSize = true;
            this.btnOpenDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenDir.Location = new System.Drawing.Point(91, 18);
            this.btnOpenDir.Name = "btnOpenDir";
            this.btnOpenDir.Size = new System.Drawing.Size(61, 25);
            this.btnOpenDir.TabIndex = 14;
            this.btnOpenDir.Text = "Open Dir";
            this.btnOpenDir.UseVisualStyleBackColor = true;
            this.btnOpenDir.Click += new System.EventHandler(this.btnOpenDir_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.btnStop, 2);
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Location = new System.Drawing.Point(158, 80);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(148, 25);
            this.btnStop.TabIndex = 13;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnOpenComposer
            // 
            this.btnOpenComposer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.btnOpenComposer, 2);
            this.btnOpenComposer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenComposer.Location = new System.Drawing.Point(158, 18);
            this.btnOpenComposer.Name = "btnOpenComposer";
            this.btnOpenComposer.Size = new System.Drawing.Size(148, 25);
            this.btnOpenComposer.TabIndex = 1;
            this.btnOpenComposer.Text = "Open Composer";
            this.btnOpenComposer.UseVisualStyleBackColor = true;
            this.btnOpenComposer.Click += new System.EventHandler(this.btnOpenComposer_Click);
            // 
            // btnClearData
            // 
            this.btnClearData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearData.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.btnClearData, 2);
            this.btnClearData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearData.Location = new System.Drawing.Point(18, 80);
            this.btnClearData.Name = "btnClearData";
            this.btnClearData.Size = new System.Drawing.Size(134, 25);
            this.btnClearData.TabIndex = 12;
            this.btnClearData.Text = "Clear Data";
            this.btnClearData.UseVisualStyleBackColor = true;
            this.btnClearData.Click += new System.EventHandler(this.btnClearData_Click);
            // 
            // btnOpenGIF
            // 
            this.btnOpenGIF.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenGIF.AutoSize = true;
            this.btnOpenGIF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenGIF.Location = new System.Drawing.Point(18, 18);
            this.btnOpenGIF.Name = "btnOpenGIF";
            this.btnOpenGIF.Size = new System.Drawing.Size(67, 25);
            this.btnOpenGIF.TabIndex = 0;
            this.btnOpenGIF.Text = "Open GIF";
            this.btnOpenGIF.UseVisualStyleBackColor = true;
            this.btnOpenGIF.Click += new System.EventHandler(this.btnOpenGIF_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(18, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 26);
            this.label2.TabIndex = 7;
            this.label2.Text = "Follow Delay";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPatternFollowDelay
            // 
            this.numPatternFollowDelay.Location = new System.Drawing.Point(91, 111);
            this.numPatternFollowDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPatternFollowDelay.Name = "numPatternFollowDelay";
            this.numPatternFollowDelay.Size = new System.Drawing.Size(61, 20);
            this.numPatternFollowDelay.TabIndex = 8;
            this.numPatternFollowDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numPatternFollowDelay.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numPatternFollowDelay.ValueChanged += new System.EventHandler(this.numPatternFollowDelay_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(158, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 26);
            this.label1.TabIndex = 6;
            this.label1.Text = "Follow Interval";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(18, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 31);
            this.label4.TabIndex = 11;
            this.label4.Text = "Frame #";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numFrame
            // 
            this.numFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numFrame.Location = new System.Drawing.Point(91, 49);
            this.numFrame.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrame.Name = "numFrame";
            this.numFrame.Size = new System.Drawing.Size(61, 20);
            this.numFrame.TabIndex = 3;
            this.numFrame.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numFrame.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrame.ValueChanged += new System.EventHandler(this.numFrame_ValueChanged);
            // 
            // btnRunAll
            // 
            this.btnRunAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.btnRunAll, 2);
            this.btnRunAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunAll.Location = new System.Drawing.Point(158, 49);
            this.btnRunAll.Name = "btnRunAll";
            this.btnRunAll.Size = new System.Drawing.Size(148, 25);
            this.btnRunAll.TabIndex = 4;
            this.btnRunAll.Text = "Run All";
            this.btnRunAll.UseVisualStyleBackColor = true;
            this.btnRunAll.Click += new System.EventHandler(this.btnRunAll_Click);
            // 
            // numPatternFollowInterval
            // 
            this.numPatternFollowInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numPatternFollowInterval.Location = new System.Drawing.Point(239, 111);
            this.numPatternFollowInterval.Maximum = new decimal(new int[] {
            276447232,
            23283,
            0,
            0});
            this.numPatternFollowInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPatternFollowInterval.Name = "numPatternFollowInterval";
            this.numPatternFollowInterval.Size = new System.Drawing.Size(67, 20);
            this.numPatternFollowInterval.TabIndex = 5;
            this.numPatternFollowInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numPatternFollowInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPatternFollowInterval.ValueChanged += new System.EventHandler(this.numPatternFollowInterval_ValueChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 149);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "SEYR Desktop   v1.0";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPatternFollowDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPatternFollowInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnOpenGIF;
        private System.Windows.Forms.Button btnOpenComposer;
        private System.Windows.Forms.NumericUpDown numFrame;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnRunAll;
        private System.Windows.Forms.NumericUpDown numPatternFollowInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numPatternFollowDelay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClearData;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnOpenDir;
    }
}

