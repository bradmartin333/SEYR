namespace SEYR.ImageProcessing
{
    partial class ParameterEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParameterEntry));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.PBX = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnAutoSettings = new System.Windows.Forms.Button();
            this.BtnClearMasks = new System.Windows.Forms.Button();
            this.BtnContinue = new System.Windows.Forms.Button();
            this.TrackbarThreshold = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.NumScaling = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackbarThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumScaling)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel.Controls.Add(this.PBX, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.BtnAutoSettings, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.BtnClearMasks, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.BtnContinue, 2, 5);
            this.tableLayoutPanel.Controls.Add(this.TrackbarThreshold, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.NumScaling, 1, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 6;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(434, 461);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // PBX
            // 
            this.PBX.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tableLayoutPanel.SetColumnSpan(this.PBX, 3);
            this.PBX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PBX.Location = new System.Drawing.Point(3, 3);
            this.PBX.Name = "PBX";
            this.PBX.Size = new System.Drawing.Size(428, 350);
            this.PBX.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PBX.TabIndex = 0;
            this.PBX.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 394);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 28);
            this.label1.TabIndex = 1;
            this.label1.Text = "Threshold";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BtnAutoSettings
            // 
            this.BtnAutoSettings.BackColor = System.Drawing.Color.White;
            this.BtnAutoSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnAutoSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAutoSettings.Location = new System.Drawing.Point(3, 435);
            this.BtnAutoSettings.Name = "BtnAutoSettings";
            this.BtnAutoSettings.Size = new System.Drawing.Size(138, 23);
            this.BtnAutoSettings.TabIndex = 6;
            this.BtnAutoSettings.Text = "Auto Settings";
            this.BtnAutoSettings.UseVisualStyleBackColor = false;
            this.BtnAutoSettings.Click += new System.EventHandler(this.BtnAutoSettings_Click);
            // 
            // BtnClearMasks
            // 
            this.BtnClearMasks.BackColor = System.Drawing.Color.White;
            this.BtnClearMasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnClearMasks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClearMasks.Location = new System.Drawing.Point(147, 435);
            this.BtnClearMasks.Name = "BtnClearMasks";
            this.BtnClearMasks.Size = new System.Drawing.Size(138, 23);
            this.BtnClearMasks.TabIndex = 7;
            this.BtnClearMasks.Text = "Clear Masks";
            this.BtnClearMasks.UseVisualStyleBackColor = false;
            this.BtnClearMasks.Click += new System.EventHandler(this.BtnClearMasks_Click);
            // 
            // BtnContinue
            // 
            this.BtnContinue.BackColor = System.Drawing.Color.White;
            this.BtnContinue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnContinue.Location = new System.Drawing.Point(291, 435);
            this.BtnContinue.Name = "BtnContinue";
            this.BtnContinue.Size = new System.Drawing.Size(140, 23);
            this.BtnContinue.TabIndex = 8;
            this.BtnContinue.Text = "Continue";
            this.BtnContinue.UseVisualStyleBackColor = false;
            this.BtnContinue.Click += new System.EventHandler(this.BtnContinue_Click);
            // 
            // TrackbarThreshold
            // 
            this.tableLayoutPanel.SetColumnSpan(this.TrackbarThreshold, 2);
            this.TrackbarThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TrackbarThreshold.Location = new System.Drawing.Point(147, 397);
            this.TrackbarThreshold.Maximum = 255;
            this.TrackbarThreshold.Name = "TrackbarThreshold";
            this.TrackbarThreshold.Size = new System.Drawing.Size(284, 22);
            this.TrackbarThreshold.TabIndex = 9;
            this.TrackbarThreshold.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TrackbarThreshold.Value = 170;
            this.TrackbarThreshold.Scroll += new System.EventHandler(this.TrackbarThreshold_Scroll);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 366);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(138, 28);
            this.label6.TabIndex = 14;
            this.label6.Text = "Scaling";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumScaling
            // 
            this.NumScaling.DecimalPlaces = 2;
            this.NumScaling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NumScaling.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NumScaling.Location = new System.Drawing.Point(147, 369);
            this.NumScaling.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumScaling.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumScaling.Name = "NumScaling";
            this.NumScaling.Size = new System.Drawing.Size(138, 20);
            this.NumScaling.TabIndex = 15;
            this.NumScaling.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumScaling.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.NumScaling.ValueChanged += new System.EventHandler(this.NumScaling_ValueChanged);
            // 
            // ParameterEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 461);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(450, 500);
            this.Name = "ParameterEntry";
            this.Text = "Custom Filter Parameter Entry";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackbarThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumScaling)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox PBX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnAutoSettings;
        private System.Windows.Forms.Button BtnClearMasks;
        private System.Windows.Forms.Button BtnContinue;
        private System.Windows.Forms.TrackBar TrackbarThreshold;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown NumScaling;
    }
}